using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { UserRole.Admin, UserRole.User };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var adminUser = new ApplicationUser
                {
                    FirstName = "Max",
                    LastName = "Sheludchenko",
                    UserName = "xpert",
                    Email = "maxsheludchenko@gmail.com",
                    PhoneNumber = "+380952099565",
                    Role = UserRole.Admin,
                    RegisteredAt = DateTime.UtcNow,
                    BirthDate = new DateOnly(2004, 2, 3)
                };

                var normalUser = new ApplicationUser
                {
                    FirstName = "Maksimka",
                    LastName = "Buster",
                    UserName = "paradice",
                    Email = "paradicelocal@gmail.com",
                    PhoneNumber = "+380507777777",
                    Role = UserRole.User,
                    RegisteredAt = DateTime.UtcNow,
                    BirthDate = new DateOnly(2001, 12, 15)
                };

                if (userManager.Users.All(u => u.UserName != adminUser.UserName))
                {
                    await userManager.CreateAsync(adminUser, "aboba123");
                    await userManager.AddToRoleAsync(adminUser, UserRole.Admin);
                }

                if (userManager.Users.All(u => u.UserName != normalUser.UserName))
                {
                    await userManager.CreateAsync(normalUser, "bebra123");
                    await userManager.AddToRoleAsync(normalUser, UserRole.User);
                }
            }
        }
    }
}
