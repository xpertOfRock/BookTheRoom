﻿using Microsoft.Extensions.DependencyInjection;
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
                cfg => cfg.RegisterServicesFromAssembly(assembly)
            );

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
