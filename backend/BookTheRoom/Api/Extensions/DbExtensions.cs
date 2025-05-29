using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class DbExtensions
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}
