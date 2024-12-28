using Application.UseCases.Commands.Apartment;
using Application.UseCases.Commands.Comment;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Commands.Order;
using Application.UseCases.Commands.Room;
using Application.UseCases.Queries.Apartment;
using Application.UseCases.Queries.Hotel;
using Application.UseCases.Queries.Order;
using Application.UseCases.Queries.Room;
using Application.UseCases.Validators.Address;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.DependencyInjection
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR
            (
                cfg => cfg.RegisterServicesFromAssemblies
                (
                    Assembly.GetExecutingAssembly()
                )
            );

            services.AddScoped<IValidator<Address>, AddressValidator>();

            services.AddValidatorsFromAssemblies(
                [
                    Assembly.GetExecutingAssembly ()
                    //typeof(CreateHotelCommand).Assembly,
                    //typeof(UpdateHotelCommand).Assembly,

                    //typeof(CreateRoomCommand).Assembly,
                    //typeof(UpdateRoomCommand).Assembly,

                    //typeof(CreateApartmentCommand).Assembly
                    ////typeof(UpdateApartmentCommand).Assembly,              

                    ////typeof(CreateOrderCommand).Assembly
                ]
            );
            return services;
        }
    }
}
