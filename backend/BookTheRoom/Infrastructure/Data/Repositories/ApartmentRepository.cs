using Application.Interfaces;
using Core.Contracts;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IPhotoService _photoService;
        private readonly ApplicationDbContext _context;

        public ApartmentRepository(ApplicationDbContext context, IMemoryCache memoryCache, IPhotoService photoService)
        {
            _context = context;
            _memoryCache = memoryCache;
            _photoService = photoService;
        }
        public async Task Add(Apartment apartment)
        {
            await _context.Apartments.AddAsync(apartment);
        }

        public async Task Delete(int id)
        {
            var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id);

            _memoryCache.Remove($"hotel-{id}");

            if (apartment.Images != null && apartment.Images.Count > 0)
            {
                foreach (var image in apartment.Images)
                {
                    await _photoService.DeletePhotoAsync(image);
                }
            }

            _context.Apartments.Remove(apartment);
        }

        public async Task<List<Apartment>> GetAll(GetDataRequest request)
        {
            var query = _context.Apartments
                .Include(h => h.Address)                
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||                            
                            h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.City.ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            Expression<Func<Apartment, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "name" => apartment => apartment.Title,
                _ => apartment => apartment.Id
            };

            query = request.SortOrder == "desc"
                 ? query = query.OrderByDescending(selectorKey)
                 : query = query.OrderBy(selectorKey);

            return await query.ToListAsync();
        }

        public async Task<Apartment> GetById(int? id)
        {
            string key = $"apartment-{id}";
            return await _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return _context.Apartments
                    .Include(h => h.Address)
                    .Include(h => h.Comments)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.Id == id);
                });
        }

        public async Task Update(int id, UpdateApartmentRequest request)
        {
            string key = $"hotel-{id}";

            var apartment = await GetById(id);

            var comments = request.Comments == null ? apartment.Comments : new List<Comment>();

            if (request.Images is not null)
            {
                if (apartment.Images != null && apartment.Images.Any())
                {
                    foreach (var image in apartment.Images)
                    {
                        await _photoService.DeletePhotoAsync(image);
                    }
                }

                await _context.Apartments
                        .Where(h => h.Id == id)
                        .ExecuteUpdateAsync(e => e
                        .SetProperty(h => h.Images, request.Images));
            }


            await _context.Apartments
                .Where(h => h.Id == id)
                .ExecuteUpdateAsync(e => e
                .SetProperty(h => h.Title, request.Title)
                .SetProperty(h => h.Description, request.Description)
                .SetProperty(h => h.PriceForNight, request.Price));

            _memoryCache.Set(key, apartment, TimeSpan.FromMinutes(2));
        }
    }
}
