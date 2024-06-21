using BookTheRoom.Application.UseCases.Commands.Address;
using BookTheRoom.Application.UseCases.Commands.Hotel;
using BookTheRoom.Application.UseCases.Commands.Order;
using BookTheRoom.Application.UseCases.Commands.Room;
using BookTheRoom.Application.UseCases.Queries.Address;
using BookTheRoom.Application.UseCases.Queries.Hotel;
using BookTheRoom.Application.UseCases.Queries.Order;
using BookTheRoom.Application.UseCases.Queries.Room;
using Microsoft.Extensions.DependencyInjection;

namespace BookTheRoom.Application.DependencyInjection
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(

                typeof(GetAllHotelsQuery).Assembly,
                typeof(GetHotelByIdQuery).Assembly,
                typeof(CreateHotelCommand).Assembly,
                typeof(UpdateHotelCommand).Assembly,
                typeof(DeleteHotelCommand).Assembly,

                typeof(GetAllAddressesQuery).Assembly,
                typeof(GetAddressByPropertiesQuery).Assembly,
                typeof(CreateAddressCommand).Assembly,
                typeof(UpdateAddressCommand).Assembly,
                typeof(DeleteAddressCommand).Assembly,

                typeof(CreateOrderCommand).Assembly,
                typeof(UpdateOrderCommand).Assembly,
                typeof(GetAllOrdersQuery).Assembly,
                typeof(GetExpiredOrdersQuery).Assembly,
                typeof(GetOrderByIdQuery).Assembly,
                typeof(GetActiveOrdersQuery).Assembly,
                typeof(GetUserOrdersQuery).Assembly,

                typeof(CreateRoomCommand).Assembly,
                typeof(DeleteRoomCommand).Assembly,
                typeof(UpdateRoomCommand).Assembly,
                typeof(GetAllRoomsQuery).Assembly,
                typeof(GetRoomByIdQuery).Assembly,
                typeof(GetRoomByNumberQuery).Assembly,
                typeof(GetRoomsByHotelQuery).Assembly
            ));

            return services;
        }
    }
}
