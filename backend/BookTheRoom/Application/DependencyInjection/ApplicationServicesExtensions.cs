using Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.DependencyInjection
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR
            (
                config =>
                {
                    config.RegisterServicesFromAssembly(assembly);
                    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                }
            );

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
