using Application.Interfaces;
using Core.Contracts;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.TasksResults;
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

        private const int maxItemsOnPage = 15;

        public async Task<IResult> Add(Hotel hotel)
        {
            Hotel? existingHotel = await _context.Hotels.AsNoTracking().FirstOrDefaultAsync(h => h.Name == hotel.Name);

            if(existingHotel is not null)
            {
                return new Fail("Entity with this address and name already exists.");
            }

            await _context.Hotels.AddAsync(hotel);

            return new Success("Entity 'Hotel' was created successfuly.");
        }
        
        public async Task<IResult> Delete(int id)
        {            
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);

            if(hotel == null)
            {
                return new Fail("Impossible to delete a non-existent entity.");
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
            return new Success("Entity 'Hotel' was deleted successfuly.");
        }
        
        public async Task<List<Hotel>> GetAll(GetHotelsRequest request)
        {
            var query = _context.Hotels
                .Include(h => h.Address)
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||
                            h.Name.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.City.ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Countries))
            {
                var countries = request.Countries.Split(',');
                query = query.Where(h => countries.Contains(h.Address.Country));
            }

            if (!string.IsNullOrWhiteSpace(request.Ratings))
            {
                var ratings = request.Ratings.Split(',').Select(int.Parse).ToList();
                query = query.Where(h => ratings.Contains(h.Rating));
            }

            if (!string.IsNullOrWhiteSpace(request.Services))
            {
                var services = request.Services.Split(',').Select(service => Enum.Parse<HotelService>(service, true));
                query = query.Where(h => h.Services.Any(x => services.Contains(x.ServiceName)));
            }

            Expression<Func<Hotel, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "name" => hotel => hotel.Name,
                "rating" => hotel => hotel.Rating,
                _ => hotel => hotel.Id
            };

            query = request.SortOrder == "desc"
                ? query.OrderByDescending(selectorKey)
                : query.OrderBy(selectorKey);

            query = query.Skip((request.page - 1) * maxItemsOnPage).Take(maxItemsOnPage);

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
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.Id == id);
                });
        }

        public async Task<IResult> Update(int id, UpdateHotelRequest request)
        {
            var hotel = await GetById(id);

            if (hotel == null)
            {
                return new Fail("Impossible to update a non-existent entity.");
            }

            string key = $"hotel-{id}";

            _memoryCache.Remove(key);


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
                .SetProperty(h => h.Rating, (int)request.Rating)
                .SetProperty(h => h.HasPool, request.HasPool)
                /*.SetProperty(h => h.Comments, request.Comments)*/);

            return new Success("Entity 'Hotel' was updated successfuly.");
        }
    }
}
