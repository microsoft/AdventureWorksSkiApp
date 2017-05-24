using AdventureWorks.SkiResort.Infrastructure.AzureSearch;
using AdventureWorks.SkiResort.Infrastructure.Infraestructure;
using AdventureWorks.SkiResort.Infrastructure.Repositories;
using AdventureWorks.SkiResort.Infrastructure.CosmosDB.Repositories;
using AdventureWorks.SkiResort.Web.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdventureWorks.SkiResort.Web.AppBuilderExtensions
{
    public static class DependenciesExtensions
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddTransient<AuthorizationProvider>();

            services.AddScoped<IConfigurationRoot>(c => { return configuration; });
            services.AddScoped<LiftsRepository>();
            services.AddScoped<RestaurantsRepository>();
            services.AddScoped<RestaurantsSearchService>();
            services.AddScoped<RentalsRepository>();
            services.AddScoped<SummariesRepository>();
            services.AddScoped<SkiResortDataInitializer>();
            services.AddScoped<AzureSearchDataInitializer>();
            services.AddScoped<UsersRepository>();
            services.AddScoped<LiftLinesRepository>();

            return services;
        }
    }
}