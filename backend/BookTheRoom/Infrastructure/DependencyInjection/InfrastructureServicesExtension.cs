using Application.ExternalServices;
using Infrastructure.Data.BackgroundServices;
using Infrastructure.Data.BackgroundServices.Services;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();

            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IOrderStatusUpdaterService, OrderStatusUpdaterService>();
            services.AddHostedService<OrderStatusUpdaterBackgroundService>();

            return services;
        }
    }
}
