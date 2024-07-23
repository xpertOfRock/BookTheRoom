using Application.ExternalServices;
using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Commands.Order;
using Application.UseCases.Commands.Room;
using Application.UseCases.Queries.Hotel;
using Application.UseCases.Queries.Order;
using Application.UseCases.Queries.Room;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationMediatr(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(

                typeof(GetHotelsQuery).Assembly,
                typeof(GetHotelQuery).Assembly,
                typeof(CreateHotelCommand).Assembly,
                typeof(UpdateHotelCommand).Assembly,
                typeof(DeleteHotelCommand).Assembly,

                typeof(CreateRoomCommand).Assembly,
                typeof(DeleteRoomCommand).Assembly,
                typeof(UpdateRoomCommand).Assembly,
                typeof(GetRoomsQuery).Assembly,
                typeof(GetHotelRoomsQuery).Assembly,
                typeof(GetRoomQuery).Assembly,

                typeof(CreateOrderCommand).Assembly,
                typeof(UpdateOrderCommand).Assembly,
                typeof(GetAllOrdersQuery).Assembly,
                typeof(GetExpiredOrdersQuery).Assembly,
                //typeof(GetOrderQuery).Assembly,
                typeof(GetActiveOrdersQuery).Assembly,
                typeof(GetUserOrdersQuery).Assembly
                )
            );

            return services;
        }
    }
}
