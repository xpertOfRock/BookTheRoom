﻿using BookTheRoom.Core.Entities;
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

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public ICollection<Order>? Orders { get; set; }
    }
}
