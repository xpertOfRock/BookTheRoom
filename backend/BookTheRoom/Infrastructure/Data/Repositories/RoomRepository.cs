using Application.Interfaces;
using CloudinaryDotNet.Actions;
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
            var room = await _context.Rooms.FindAsync(hotelId, number);

            _memoryCache.Remove($"hotel-{hotelId}-room-{number}");

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
            return await _context.Rooms.AsNoTracking().ToListAsync();
        }
        public async Task<List<Room>> GetAll(int hotelId, GetRoomsRequest request)
        {
            var query = _context.Rooms
                .Where(r => string.IsNullOrWhiteSpace(request.Search) ||
                            r.Category.ToString().ToLower().Contains(request.Search.ToLower()) ||
                            r.HotelId == hotelId)
                .AsNoTracking();

            if (!query.Any())
            {
                return null;
            }

            Expression<Func<Room, object>> selectorKey = request.SortItem.ToLower() switch
            {
                "categoty" => room => room.Category.ToString(),
                "price" => room => room.Price,
                _ => room => room.Number,
            };

            if (request.SortOrder == "desc")
            {
                query = query.OrderByDescending(selectorKey);
            }
            else
            {
                query = query.OrderBy(selectorKey);
            }


            return await query.ToListAsync();
        }

        public async Task<Room> GetById(int hotelId, int number)
        {
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
            string key = $"hotel-{hotelId}-room-{number}";

            await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.Number == number)
                .ExecuteUpdateAsync(e => e
                .SetProperty(r => r.Name, request.Name)
                .SetProperty(r => r.Description, request.Description)
                .SetProperty(r => r.Price, request.Price)
                .SetProperty(r => r.Images, request.Images)
                .SetProperty(r => r.Category, request.Category)
                );

            var room = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.HotelId == hotelId &&
                    r.Number == number
                );

            _memoryCache.Set(key, room, TimeSpan.FromMinutes(2));
        }
    }
}
