using BookTheRoom.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace BookTheRoom.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Telegram { get; set; }
        public string? XLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? FacebookLink { get; set; }
        public string? ImageURL { get; set; }        
        public int AddressId { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public Address Address { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
