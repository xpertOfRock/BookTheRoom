using BookTheRoom.Core.Interfaces;
using BookTheRoom.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;
        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Address address)
        {
            await _context.Addresses.AddAsync(address);
        }

        public void Delete(Address address)
        {
            _context.Addresses.Remove(address);
        }

        public async Task<Address> GetAddress(string country, string city, string streetOrDistrict, int index)
        {
            return await _context.Addresses.AsNoTracking().Where(a =>  a.Country == country &&
                                                                       a.City == city &&
                                                                       a.StreetOrDistrict == streetOrDistrict &&
                                                                       a.Index == index).FirstAsync();         
        }

        public async Task<List<Address>> GetAllAsync()
        {
            return await _context.Addresses.AsNoTracking().ToListAsync();
        }

        public void Update(Address address)
        {
            _context.Addresses.Update(address);
        }
    }
}
