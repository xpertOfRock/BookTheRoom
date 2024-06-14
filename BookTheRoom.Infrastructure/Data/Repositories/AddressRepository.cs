using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;


namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context) : base(context)
        {

        }        
    }
}
