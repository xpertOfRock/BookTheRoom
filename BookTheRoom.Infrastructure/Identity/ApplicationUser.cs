using BookTheRoom.Domain.Entities;
using BookTheRoom.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;


namespace BookTheRoom.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {         
        public int AddressId { get; set; }

        public Address Address { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
