using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context) : base(context){}
        public async Task<List<Room>> GetAllRoomsByHotel(int id)
        {
            return await ApplicationDbContext.Rooms.Where(r => r.HotelId == id).ToListAsync();
        }
        public async Task<Room> GetRoomByIdAsync(int hotelId, int id)
        {
            return await ApplicationDbContext.Rooms.Where(r => r.Id == id && r.HotelId == hotelId).FirstOrDefaultAsync();
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
