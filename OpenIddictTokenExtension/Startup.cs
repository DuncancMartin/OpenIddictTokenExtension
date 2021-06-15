using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Threading.Tasks;
using System.Threading;
using System;
using OpenIddict.Abstractions;
using IdentityApiOpenIddict_TokenExtension;
using IdentityApiOpenIddict_TokenExtension.Models;
using OpenIddictTokenExtension.DataAccessLayer.Interfaces;
using OpenIddictTokenExtension.DataAccessLayer;

namespace OpenIddictTokenExtension
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetValue<string>("IdentityDbConnectionString"));
                options.UseOpenIddict<CustomApplication, CustomAuthorization, CustomScope, ExtendedOpenIddictEntityFrameworkCoreToken, long>();
            });

            var redisHost = Configuration.GetValue<string>("RedisHost");
            var redisPort = Configuration.GetValue<int>("RedisPort");
            var redis = ConnectionMultiplexer.Connect($"{redisHost}:{redisPort}");

            services.AddDataProtection()
                .SetApplicationName("Imagine")
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

            services.AddImagineIdentityConfig();
            services.AddAuthentication();

            services.AddControllers();

            services.AddCors();
            services.AddAutoMapper(typeof(Startup));


            services.AddScoped<IDataContextFactory, DataContextFactory>();

            var container = services.BuildServiceProvider();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //This will create the DB if it does not exist.
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                context.Database.Migrate();
            }

            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //  Eventually only allow from k3 approved sources
            //  IHostingEnvironment can be used to further limit access
            app.UseCors(o => o.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(i => i.MapControllers());
            AddPublicapiToApplicationDatabaseAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
        }

        private async Task AddPublicapiToApplicationDatabaseAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("publicApi", cancellationToken) == null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "publicApi",
                    DisplayName = "Public API",
                    PostLogoutRedirectUris = { new Uri("https://oidcdebugger.com/debug") },
                    RedirectUris = { new Uri("https://oidcdebugger.com/debug") },
                    ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342"
                };

                await manager.CreateAsync(descriptor, cancellationToken);
            }
        }

    }
}