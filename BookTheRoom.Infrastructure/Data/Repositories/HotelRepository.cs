using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<Hotel> GetHotelByAddress(string country, string city, string street, int building)
        {
            return await ApplicationDbContext.Hotels.
                         Where(h => h.Address.Country == country &&
                                    h.Address.City == city &&
                                    h.Address.Street == street &&
                                    h.Address.Building == building).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Hotel>> GetAllHotelsByAddress(string country, string? city, string? street, int? building)
        {
            var query = ApplicationDbContext.Hotels.Where(h => h.Address.Country == country);

            if (city != null)
                query = query.Where(h => h.Address.City == city);
            if (street != null)
                query = query.Where(h => h.Address.Street == street);
            if (building != null)
                query = query.Where(h => h.Address.Building == building);

            return await query.ToListAsync();
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
