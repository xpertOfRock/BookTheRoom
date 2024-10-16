using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Service service)
        {
            await _context.Services.AddAsync(service);
        }

        public async Task<List<Service>> GetAll(int hotelId)
        {
            return await _context.Services
                .Where(x => x.HotelId == hotelId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Service> GetService(int hotelId, OrderOptionsName optionsName)
        {
            return await _context.Services.FirstOrDefaultAsync(x => x.HotelId == hotelId && x.OptionName == optionsName);
        }

        public async Task Update(int hotelId, OrderOptionsName optionName, decimal price)
        {
            await _context.Services
                .Where(x => x.HotelId == hotelId && x.OptionName == optionName)
                .ExecuteUpdateAsync(x =>
                x.SetProperty(p => p.OptionPrice, price));
        }
    }
}
