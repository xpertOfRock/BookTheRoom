using Application;
using Application.DependencyInjection;
using Application.ExternalServices;
using Application.Interfaces;
using Application.Settings;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Data.BackgroundServices;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IApartmentRepository, ApartmentRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddHostedService<OrderStatusUpdater>();
builder.Services.AddHostedService<RoomStatusUpdater>();

builder.Services.Configure<HostOptions>(options =>
{
    options.ServicesStartConcurrently = true;
    options.ServicesStopConcurrently = false;
});

builder.Services.AddApplicationMediatr();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddMemoryCache();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 5;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddHttpClient();

builder.Services.AddCors(options =>

    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:3000", "https://localhost:5275");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    })
);

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.Configure<BraintreeSettings>(builder.Configuration.GetSection("BraintreeGateway"));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<HostOptions>(options =>
{
    options.ServicesStartConcurrently = true;
    options.ServicesStopConcurrently = false;
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.Authority = "https://localhost:5275"; 
//    options.Audience = "api"; 
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateAudience = true,
//        ValidateIssuer = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true
//    };
//});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await SeedData.Initialize(services);
//}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
