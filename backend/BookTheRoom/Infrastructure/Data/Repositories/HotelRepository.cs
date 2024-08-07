using Application.Interfaces;
using Core.Contracts;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Tls;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IPhotoService _photoService;
        private readonly ApplicationDbContext _context;
        
        public HotelRepository(ApplicationDbContext context, IMemoryCache memoryCache, IPhotoService photoService)
        {
            _context = context;
            _memoryCache = memoryCache;
            _photoService = photoService;
        }
        public async Task Add(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
        }

        public async Task Delete(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            _memoryCache.Remove($"hotel-{id}");

            if (hotel.Images != null && hotel.Images.Count > 0)
            {
                foreach (var image in hotel.Images)
                {
                    await _photoService.DeletePhotoAsync(image);
                }
            }
            
            _context.Hotels.Remove(hotel);
        }

        public async Task<List<Hotel>> GetAll(GetDataRequest request)
        {
            var query = _context.Hotels
                .Include(h => h.Address)
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||
                            h.Name.ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            Expression<Func<Hotel, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "name" => hotel => hotel.Name,
                "rating" => hotel => hotel.Rating,
                _ => hotel => hotel.Id
            };
                     
            query = request.SortOrder == "desc"
                 ? query = query.OrderByDescending(selectorKey)
                 : query = query.OrderBy(selectorKey);                    

            return await query.ToListAsync();
        }

        public async Task<Hotel> GetById(int id)
        {
            string key = $"hotel-{id}";
            return await _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return _context.Hotels
                    .Include(h => h.Address)                                         
                    .Include(h => h.Rooms)
                    .Include(h => h.Comments)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.Id == id);
                });
        }

        public async Task Update(int id, UpdateHotelRequest request)
        {
            string key = $"hotel-{id}";

            await _context.Hotels
                .Where(h => h.Id == id)
                .ExecuteUpdateAsync(e => e
                .SetProperty(h => h.Name, request.Name)
                .SetProperty(h => h.Description, request.Description)
                .SetProperty(h => h.Rating, request.Rating)
                .SetProperty(h => h.RoomsAmount, request.RoomsAmount)
                .SetProperty(h => h.HasPool, request.HasPool)
                .SetProperty(h => h.Images, request.Images)
                );

            var hotel = await _context.Hotels.FindAsync(id);

            _memoryCache.Set(key, hotel, TimeSpan.FromMinutes(2));
        }
    }
}
