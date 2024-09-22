using Application.Interfaces;
using Core.Contracts;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);

            if(hotel == null)
            {
                return;
            }

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
                            h.Name.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.City.ToLower().Contains(request.Search.ToLower())
                            )                          
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

        public async Task<Hotel> GetById(int? id)
        {
            if (id == null) throw new ArgumentNullException("Cannot get entity 'Hotel' when 'id' is null.");

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
                       
            var hotel = await GetById(id);

            if(hotel == null)
            {
                return;
            }

            var comments = !request.Comments.Any() ? hotel.Comments : new List<Comment>();

            if (request.Images is not null)
            {
                if (hotel.Images != null && hotel.Images.Any())
                {
                    foreach (var image in hotel.Images)
                    {
                        await _photoService.DeletePhotoAsync(image);
                    }
                }
                await _context.Hotels
                        .Where(h => h.Id == id)
                        .ExecuteUpdateAsync(e => e
                        .SetProperty(h => h.Images, request.Images));
            }

            await _context.Hotels
                .Where(h => h.Id == id)
                .ExecuteUpdateAsync(e => e
                .SetProperty(h => h.Name, request.Name)
                .SetProperty(h => h.Description, request.Description)
                .SetProperty(h => h.Rating, request.Rating)
                .SetProperty(h => h.HasPool, request.HasPool)
                .SetProperty(h => h.Comments, request.Comments));

            _memoryCache.Set(key, hotel, TimeSpan.FromMinutes(2));
        }
    }
}
