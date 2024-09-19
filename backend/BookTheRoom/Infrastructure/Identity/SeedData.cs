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
                    UserName = "xpert",
                    Email = "maxsheludchenko@gmail.com",
                    Role = UserRole.Admin,
                    RegisteredAt = DateTime.UtcNow
                };

                var normalUser = new ApplicationUser
                {
                    UserName = "paradice",
                    Email = "paradicelocal@gmail.com",
                    Role = UserRole.User,
                    RegisteredAt = DateTime.UtcNow
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
