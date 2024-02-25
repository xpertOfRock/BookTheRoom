using BookTheRoom.Domain.Entities;
using BookTheRoom.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BookTheRoom.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public UserRole Role { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
