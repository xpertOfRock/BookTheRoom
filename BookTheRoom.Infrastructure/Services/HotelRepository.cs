using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using BookTheRoom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookTheRoom.Infrastructure.Services
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelRepository(ApplicationDbContext context)
        {
                _context = context;
        }

        public Task Add(Hotel hotel)
        {
            _context.Add(hotel);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Delete(Hotel hotel)
        {
            _context.Remove(hotel);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
        public Task Update(Hotel hotel)
        {
            _context.Update(hotel);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
        public async Task<List<Hotel>> GetAllHotels()
        {
            return await _context.Hotels.Include(h => h.Address).ToListAsync();
        }
        public async Task<Hotel> GetHotelByAddress(Address address)
        {
            return await _context.Hotels.Where(h => h.Address == address).FirstOrDefaultAsync();
        }
        public async Task<Hotel> GetHotelByIdAsync(int id)
        {
            return await _context.Hotels.Include(h => h.Address).FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<List<Hotel>> GetAllHotelsByAddress(Address address)
        {
            return await _context.Hotels.Where(h => h.Address == address).ToListAsync();
        }
    }
}
