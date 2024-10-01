using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Image { get; set; } = "https://i.redd.it/aqtop2mfrsr91.jpg";
        public string Role { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Order>? Orders { get; set; }
        public List<Apartment>? Apartments { get; set; }

    }
}
