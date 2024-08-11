using IdentityServer;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
        });

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
        {
            config.Password.RequiredLength = 8;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("https://localhost:3000", "https://localhost:5286");
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowCredentials();
            })
         );
           
        builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            options.EmitStaticAudienceClaim = true;
        })
            .AddAspNetIdentity<ApplicationUser>()
            .AddInMemoryClients(Configuration.GetClients())
            .AddInMemoryApiResources(Configuration.GetApiResources())
            .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
            .AddDeveloperSigningCredential();

        var app = builder.Build();

        app.UseCors();

        app.UseIdentityServer();

        app.Run();
    }
}