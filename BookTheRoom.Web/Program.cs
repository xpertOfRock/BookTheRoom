using AspNetCoreRateLimit;
using BookTheRoom.Application.Settings;
using BookTheRoom.Application.Interfaces;
using BookTheRoom.Application.Services;
using BookTheRoom.Core.Interfaces;
using BookTheRoom.Infrastructure.Data;
using BookTheRoom.Infrastructure.Data.Interfaces;
using BookTheRoom.Infrastructure.Data.Repositories;
using BookTheRoom.Infrastructure.Data.BackgruondServices;
using BookTheRoom.Infrastructure.Identity;
using BookTheRoom.WebUI.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.Cookies;
using BookTheRoom.Application.DependencyInjection;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptions();
        builder.Services.AddControllersWithViews();
        builder.Services.AddHostedService<OrderStatusUpdaterBackgroundService>();
        builder.Services.AddHostedService<RoomStatusUpdaterBackgroundService>();

        

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IHotelRepository, HotelRepository>();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IPhotoService, PhotoService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        builder.Services.AddApplicationServices();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
      
        builder.Services.AddMemoryCache();

        builder.Services.AddSession();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie();

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
        {
            config.Password.RequiredLength = 5;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
        });

        builder.Services.AddHttpClient();

        builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

        builder.Services.Configure<BraintreeSettings>(builder.Configuration.GetSection("BraintreeGateway"));

        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

        builder.Services.Configure<HostOptions>(options =>
        {
            options.ServicesStartConcurrently = true;
            options.ServicesStopConcurrently = false;
        });

        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

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
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                Seed.SeedData(context);
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

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseEndpoints(endpoints =>
        {
            EndpointsConfiguration.ConfigureEndpoints(endpoints);
        });

        app.Run();
    }
}