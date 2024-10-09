using Application.Interfaces;
using Core.Contracts;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IPhotoService _photoService;
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context, IMemoryCache memoryCache, IPhotoService photoService)
        {
            _context = context;
            _memoryCache = memoryCache;
            _photoService = photoService;
        }
        public async Task Add(Room room)
        {
            await _context.Rooms.AddAsync(room);
        }

        public async Task Delete(int hotelId, int number)
        {
            var room = await GetById(hotelId, number);

            if(room == null)
            {
                return;
            }

            string key = $"hotel-{hotelId}-room-{number}";

            _memoryCache.Remove(key);

            if (room.Images != null && room.Images.Count > 0)
            {
                foreach (var image in room.Images)
                {
                    await _photoService.DeletePhotoAsync(image);
                }
            }

            _context.Rooms.Remove(room);
        }
        public async Task<List<Room>> GetAllRooms()
        {
            return await _context.Rooms
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Room>> GetAll(int hotelId, GetDataRequest request)
        {
            var query = _context.Rooms
                .Where(r => r.HotelId == hotelId && 
                            (string.IsNullOrWhiteSpace(request.Search) ||
                            r.Name.ToLower().Contains(request.Search.ToLower()) ) 
                            )
                .AsNoTracking();

            Expression<Func<Room, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "price" => room => room.Price,
                _ => room => room.Name
            };

            query = request.SortOrder == "desc"
                 ? query = query.OrderByDescending(selectorKey)
                 : query = query.OrderBy(selectorKey);

            return await query.ToListAsync();
        }

        public async Task<Room> GetById(int? hotelId, int? number)
        {
            if (hotelId == null || number == null) throw new ArgumentNullException("Cannot get entity 'Hotel' when 'id' is null.");

            string key = $"hotel-{hotelId}-room-{number}";
            
            return await _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return _context.Rooms
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        r => r.HotelId == hotelId &&
                        r.Number == number
                        );
                });
        }

        public async Task Update(int hotelId, int number, UpdateRoomRequest request)
        {
            var room = await GetById(hotelId, number);

            if (room == null)
            {
                return;
            }

            string key = $"hotel-{hotelId}-room-{number}";

            _memoryCache.Remove(key);

            if (request.Images is not null)
            {
                if (room.Images != null && room.Images.Any())
                {
                    foreach (var image in room.Images)
                    {
                        await _photoService.DeletePhotoAsync(image);
                    }
                }
                await _context.Rooms
                        .Where(h => h.HotelId == hotelId && h.Number == number)
                        .ExecuteUpdateAsync(e => e
                        .SetProperty(h => h.Images, request.Images));
            }

            await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.Number == number)
                .ExecuteUpdateAsync(e => e
                .SetProperty(r => r.Name, request.Name)
                .SetProperty(r => r.Description, request.Description)
                .SetProperty(r => r.Price, request.Price)
                .SetProperty(r => r.Category, request.Category));
        }
    }
}
