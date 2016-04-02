using AdventureWorks.SkiResort.Infrastructure.Context;
using AdventureWorks.SkiResort.Infrastructure.Infraestructure;
using AdventureWorks.SkiResort.Infrastructure.Model;
using AdventureWorks.SkiResort.Web.AppBuilderExtensions;
using AdventureWorks.SkiResort.Web.Infrastructure;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AdventureWorks.SkiResort.Web
{
    public class Startup
    {
        private readonly Platform _Platform;
        private readonly IApplicationEnvironment _environment;

        public Startup(IHostingEnvironment env, IRuntimeEnvironment runtimeEnvironment, IApplicationEnvironment appEnv)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
            _environment = appEnv;
            _Platform = new Platform(runtimeEnvironment);
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var useInMemoryStore = !_Platform.IsRunningOnWindows || _Platform.IsRunningOnMono || _Platform.IsRunningOnNanoServer;

            services.ConfigureDataContext(Configuration, useInMemoryStore);

            // Register dependencies
            services.ConfigureDependencies(Configuration);

            services.AddCaching();

            services.Configure<SecurityConfig>(Configuration.GetSection("Security"));

            // Add Identity services to the services container.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SkiResortContext>()
                .AddDefaultTokenProviders();

            services.AddApplicationInsightsTelemetry(Configuration);

            // Add framework services.
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(
            IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            AuthorizationProvider authorizationProvider,
            IOptions<SecurityConfig> securityConfig,
            SkiResortDataInitializer dataInitializer)
        {
            // Add Application Insights monitoring to the request pipeline as a very first middleware.
            app.UseApplicationInsightsRequestTelemetry();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Default is index 
            app.UseDefaultFiles(
                new Microsoft.AspNet.StaticFiles.DefaultFilesOptions()
                {
                    DefaultFileNames = new[] { "index.html" }
                }
            );

            app.UseIISPlatformHandler();

            // Add Application Insights exceptions handling to the request pipeline.
            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            // Basic openId.connect server
            app.UseOpenIdConnectServer(options =>
            {
                options.Provider = authorizationProvider;
                options.AllowInsecureHttp = true;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            // Jwt bearer token validation middleware
            app.UseJwtBearerAuthentication(options =>
            {
                options.Authority = securityConfig.Value.Authority;
                options.Audience = securityConfig.Value.Audience;

                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "sub"
                };

                options.AutomaticAuthenticate = true;
            });

            app.UseMiddleware<RequiredScopesMiddleware>(new List<string> { "api" });

            app.UseMvc();

            await dataInitializer.InitializeDatabaseAsync(app.ApplicationServices);
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
