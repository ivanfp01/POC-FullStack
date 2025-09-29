using Application.Services;
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

            /* Application Services */
            // LEGACY CLEANED: services.AddScoped<IDummyEntityApplicationService, DummyEntityApplicationService>();

            /* Domain Services */
            services.AddScoped<IVinService, VinService>();
            services.AddScoped<IMotorService, MotorService>();

            return services;
        }

        private static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            // Publishers for event bus will be registered here for future non-legacy events
            // LEGACY CLEANED: services.AddTransient<IIntegrationEventHandler<DummyEntityCreatedIntegrationEvent>, DummyEntityCreatedIntegrationEventHandlerPub>();
            return services;
        }

        private static IServiceCollection AddSubscribers(this IServiceCollection services)
        {
            // Subscribers for event bus will be registered here for future non-legacy events
            // LEGACY CLEANED: services.AddTransient<DummyEntityCreatedIntegrationEventHandlerSub>();
            return services;
        }
    }
}
