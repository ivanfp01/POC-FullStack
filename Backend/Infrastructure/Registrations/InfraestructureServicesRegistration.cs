using Application.Repositories;
using Core.Application;
using Core.Infraestructure;
using Domain.Others.Utils;
using Infrastructure.Constants;
using Infrastructure.Factories;
using Infrastructure.Repositories.Sql.Automoviles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using MongoDB.Bson.Serialization.Conventions;
using static Domain.Enums.Enums;

namespace Infrastructure.Registrations
{
    public static partial class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories(configuration);

            if (configuration.GetValue<bool>("EventBus:Enabled", false))
            {
                services.AddEventBus(configuration);
            }

            services.AddSingleton<IExternalApiClient, ExternalApiHttpAdapter>();

            return services;
        }

        public static IServiceCollection AddAutomovilesSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AutomovilesDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnectionAutomoviles"));
            }, ServiceLifetime.Scoped);

            services.AddScoped<IAutomovilRepository, AutomovilRepository>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            string dbType = configuration["Configurations:UseDatabase" ?? throw new NullReferenceException(InfrastructureConstants.DATABASE_TYPE_NOT_CONFIGURED)];

            services.CreateDataBase(dbType, configuration);

            return services;
        }
    }
}
