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

        private const int maxItemsOnPage = 15;

        public async Task Add(Apartment apartment)
        {
            await _context.Apartments.AddAsync(apartment);
        }

        public async Task Delete(int id)
        {
            var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id);

            if (apartment == null)
            {
                return;
            }
            var key = $"apartment-{id}";
            _memoryCache.Remove(key);

            if (apartment.Images != null && apartment.Images.Count > 0)
            {
                foreach (var image in apartment.Images)
                {
                    await _photoService.DeletePhotoAsync(image);
                }
            }

            _context.Apartments.Remove(apartment);

        }
        public async Task<List<Apartment>> GetAllUsersApartments(string userId, GetApartmentsRequest request)
        {
            var query = _context.Apartments
                .Include(h => h.Address)
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||
                            h.Title.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.City.ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Countries))
            {
                var countries = request.Countries.Split(',');
                query = query.Where(h => countries.Contains(h.Address.Country));
            }

            if (!string.IsNullOrWhiteSpace(request.Prices))
            {
                var prices = request.Prices.Split(',').Select(decimal.Parse).ToList();
                query = query.Where(h => prices[0] < h.PriceForNight && prices[1] < h.PriceForNight);
            }

            Expression<Func<Apartment, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "title" => apartment => apartment.Title,
                "price" => apartment => apartment.PriceForNight,
                _ => apartment => apartment.Id
            };

            query = request.SortOrder == "desc"
                ? query.OrderByDescending(selectorKey)
                : query.OrderBy(selectorKey);

            query = query.Skip((request.page - 1) * maxItemsOnPage).Take(maxItemsOnPage);

            return await query.ToListAsync();
        }
        public async Task<List<Apartment>> GetAll(GetApartmentsRequest request)
        {
            var query = _context.Apartments
                .Include(h => h.Address)                
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||                            
                            h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.City.ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Countries))
            {
                var countries = request.Countries.Split(',');
                query = query.Where(h => countries.Contains(h.Address.Country));
            }

            if 
            (
                !string.IsNullOrWhiteSpace(request.Prices) &&
                request.Prices.Split(',').Select(decimal.Parse).ToList().Count == 2
            )
            {
                var prices = request.Prices
                    .Split(',')
                    .Select(decimal.Parse)
                    .ToList();
                query = query.Where(h => prices[0] < h.PriceForNight && prices[1] < h.PriceForNight);
            }

            Expression<Func<Apartment, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "title" => apartment => apartment.Title,
                "price" => apartment => apartment.PriceForNight,
                _ => apartment => apartment.Id
            };


            query = request.SortOrder == "desc"
                 ? query = query.OrderByDescending(selectorKey)
                 : query = query.OrderBy(selectorKey);

            query = query.Skip((request.page - 1) * maxItemsOnPage).Take(maxItemsOnPage);

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
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.Id == id);
                });
        }

        public async Task Update(int? id, UpdateApartmentRequest request)
        {
            var apartment = await GetById(id);

            if(id is null)
            {
                throw new ArgumentNullException($"Cannot get entity '{nameof(Apartment)}' with '{id}' is null.");
            }
            string key = $"apartment-{id}";
           
            _memoryCache.Remove(key);

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
        }
    }
}
