using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Presistence.Data.DataSeed;
using E_Commerce.Presistence.Data.DbContexts;
using E_Commerce.Presistence.IdentityData.DataSeed;
using E_Commerce.Presistence.IdentityData.DbContexts;
using E_Commerce.Presistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace E_Commerce.Presistence
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            //DbContext
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            // DataSedding 
            services.AddKeyedScoped<IDataInitializer, DataInitializer>("Default");
            services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("Identity");

            ////Cach Redis
            services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            });


            //Identity db
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });
            //LightWeight than .AddIdentity<ApplicationUser,IdentityRole>()
            //used with Custom authentication and authorization Without using pre-built services provided by IdentityFrameWork
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }
    }
}
