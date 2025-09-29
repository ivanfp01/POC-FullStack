using Core.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Registrations
{
    /// <summary>
    /// Aqui se deben registrar todas las dependencias de la capa de aplicacion
    /// </summary>
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            /* Automapper */
            services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

            /* EventBus */
            services.AddPublishers();
            services.AddSubscribers();

            /* MediatR*/
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<ICommandQueryBus, MediatrCommandQueryBus>();

            return services;
        }

        private static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            // Publishers for event bus will be registered here for future non-legacy events
            return services;
        }

        private static IServiceCollection AddSubscribers(this IServiceCollection services)
        {
            // Subscribers for event bus will be registered here for future non-legacy events
            return services;
        }
    }
}
