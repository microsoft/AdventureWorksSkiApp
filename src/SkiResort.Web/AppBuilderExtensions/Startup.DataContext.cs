using AdventureWorks.SkiResort.Infrastructure.Context;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdventureWorks.SkiResort.Web.AppBuilderExtensions
{
    public static class DataContextExtensions
    {
        public static IServiceCollection ConfigureDataContext(this IServiceCollection services, IConfiguration configuration, bool useInMemoryStore)
        {
            services.AddEntityFramework()
                    .AddStore(useInMemoryStore)
                    .AddDbContext<SkiResortContext>(options =>
                    {
                        if (useInMemoryStore)
                        {
                            options.UseInMemoryDatabase();
                        }
                        else
                        {
                            options.UseSqlServer(configuration["Data:DefaultConnection:Connectionstring"]);
                        }
                    });

            return services;
        }
    }
}