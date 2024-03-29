using AspNetCoreRateLimit;
using BookTheRoom.Application.DTO;
using BookTheRoom.Application.Interfaces;
using BookTheRoom.Application.Services;
using BookTheRoom.Infrastructure.Data;
using BookTheRoom.Infrastructure.Data.Repositories;
using BookTheRoom.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptions();

        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IHotelRepository, HotelRepository>();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IPhotoService, PhotoService>();
        builder.Services.AddScoped<IBraintreeService, PaymentService>();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });       

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();


       
        builder.Services.AddMemoryCache();
        builder.Services.AddSession();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

        builder.Services.Configure<BraintreeSettings>(builder.Configuration.GetSection("BraintreeGateway"));

        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
        });

        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });


        builder.Services.AddMemoryCache();
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", fixedOptions => 
            {
                fixedOptions.PermitLimit = 1000;
                fixedOptions.Window = TimeSpan.FromSeconds(5);
            }); 
        });

        var app = builder.Build();

        if (args.Length == 1 && args[0] == "seeddata")
        {
            {
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    context.Database.EnsureCreated();

                    Seed.SeedData(context);
                    //await Seed.SeedUsersAndRolesAsync(context);
                }
            }
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");           
            app.UseHsts();
        }
        app.UseRateLimiter();
        app.UseResponseCompression();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "Rooms",
                pattern: "Hotels/Hotel/{hotelId:int}/Rooms",
                defaults: new { controller = "Hotel", action = "Rooms" }
                );

            endpoints.MapControllerRoute(
                name: "Room",
                pattern: "Hotels/Hotel/{hotelId:int}/Rooms/Room/{roomId:int}",
                defaults: new { controller = "Hotel", action = "Room" }
                );

            endpoints.MapControllerRoute(
                name: "AddHotel",
                pattern: "Hotels/Add",
                defaults: new { controller = "Hotel", action = "AddHotel" }
                );
        });

        app.Run();
    }
}