using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context) : base(context){}
        public async Task<IEnumerable<Room>> GetAllRoomsByHotel(Hotel hotel)
        {
            return await ApplicationDbContext.Rooms.Where(r => r.Hotel == hotel).ToListAsync();
        }
        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await ApplicationDbContext.Rooms.Where(r => r.Id == id).FirstOrDefaultAsync();
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
