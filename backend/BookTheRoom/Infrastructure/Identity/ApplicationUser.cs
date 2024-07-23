﻿using Core.Entities;
using Core.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Image { get; set; }
        public string Role { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;  
        public List<Order>? Orders { get; set; }

    }
}
