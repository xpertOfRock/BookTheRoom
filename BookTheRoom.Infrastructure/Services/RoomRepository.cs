using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using BookTheRoom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookTheRoom.Infrastructure.Services
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task Add(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Delete(Room room)
        {
            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<List<Room>> GetAllRoomsByHotel(Hotel hotel)
        {
            return await _context.Rooms.Where(r => r.Hotel == hotel).ToListAsync();
        }

        public async Task<Room> GetRoomByNumberAsync(int number)
        {
            return await _context.Rooms.Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Number == number);
        }

        public Task Update(Room room)
        {
            _context.Rooms.Update(room);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
