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
                                    h.Address.StreetOrDistrict == street &&
                                    h.Address.Index == building).FirstOrDefaultAsync();
        }
        public async Task<List<Hotel>> GetAllHotelsByAddress(string country, string? city, string? street, int? building)
        {
            var query = ApplicationDbContext.Hotels.Where(h => h.Address.Country == country);

            if (city != null)
                query = query.Where(h => h.Address.City == city);
            if (street != null)
                query = query.Where(h => h.Address.StreetOrDistrict == street);
            if (building != null)
                query = query.Where(h => h.Address.Index == building);

            return await query.ToListAsync();
        }

        public async Task<Hotel> GetByIdGetByIdInclude(int id)
        {
            return await ApplicationDbContext.Hotels.Include(h => h.Address)
                                                    .Include(h => h.Rooms).FirstOrDefaultAsync(h => h.Id == id);
        }

        async Task<List<Hotel>> IHotelRepository.GetAllInclude()
        {
            return await ApplicationDbContext.Hotels.Include(h => h.Address).ToListAsync();
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
