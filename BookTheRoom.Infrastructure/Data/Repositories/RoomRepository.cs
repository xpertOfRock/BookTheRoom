using BookTheRoom.Application.Interfaces;
using BookTheRoom.Application.Services;
using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly IPhotoService _photoService;
        private readonly IMemoryCache _memoryCache;
        public RoomRepository(ApplicationDbContext context, IMemoryCache memoryCache, IPhotoService photoService) : base(context)
        {
            _memoryCache = memoryCache;
            _photoService = photoService;
        }
        public async Task<List<Room>> GetAllRoomsByHotel(int id)
        {
            return await ApplicationDbContext.Rooms.Where(r => r.HotelId == id).AsNoTracking().ToListAsync();
        }
        public async Task<Room> GetRoomByNumber(int hotelId, int number)
        {
            string key = $"room-h-{hotelId}-r-{number}";

            return await _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return ApplicationDbContext.Rooms.AsNoTracking().FirstOrDefaultAsync(r => r.Number == number && r.HotelId == hotelId);
                });
        }
        public override async Task<Room> GetById(int id)
        {
            string key = $"room-{id}";

            return await _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return ApplicationDbContext.Rooms.Include(r => r.Hotel).AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                });
        }

        public override async Task Update(Room room)
        {

            string key = $"room-h-{room.HotelId}-r-{room.Number}";

            _memoryCache.Remove(key);

            ApplicationDbContext.Rooms.Update(room);

            _memoryCache.Set(key, room, TimeSpan.FromMinutes(2));
        }

        public override async Task Delete(Room room)
        {
            await _photoService.DeletePhotoAsync(room.PreviewURL);

            _memoryCache.Remove($"room-h-{room.HotelId}-r-{room.Number}");

            foreach (var item in room.ImagesURL)
            {
                await _photoService.DeletePhotoAsync(item);
            }

            ApplicationDbContext.Rooms.Remove(room);
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}
