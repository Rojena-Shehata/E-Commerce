
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Presentation.CustomeMiddleWares;
using E_Commerce.Presistence.Data.DataSeed;
using E_Commerce.Presistence.Data.DbContexts;
using E_Commerce.Presistence.IdentityData.DataSeed;
using E_Commerce.Presistence.IdentityData.DbContexts;
using E_Commerce.Presistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.MappingProfiles;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //CORs
            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("AllowAll", corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyHeader();
                    corsPolicyBuilder.AllowAnyMethod();
                    corsPolicyBuilder.AllowAnyOrigin();
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
            });

            //DbContext
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // DataSedding 
            builder.Services.AddKeyedScoped<IDataInitializer,DataInitializer>("Default");
            builder.Services.AddKeyedScoped<IDataInitializer,IdentityDataInitializer>("Identity");

            //AutoMapper
            //-version 14
            builder.Services.AddAutoMapper(typeof(ServicesAssemblyReference).Assembly);

            #region Automapper version 15
            //-In AutoMapper version 15, It doesn't work in production without licenseKey, but work in development only
            ////builder.Services.AddAutoMapper(x => x.AddProfile<ProductProfile>());
            ////builder.Services.AddTransient<ProductPictureUrlResolver>();
            //
            //-version 15 with assembly +License key+ without license key work in development only
            //// builder.Services.AddAutoMapper(x => x.LicenseKey="",typeof(ProductProfile).Assembly);

            #endregion


            /////
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IproductService, ProductService>();

            ////Cach Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();


            builder.Services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
                
            });
            //Identity db
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            //LightWeight than .AddIdentity<ApplicationUser,IdentityRole>()
            //used with Custom authentication and authorization Without using pre-built services provided by IdentityFrameWork
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.Configure<JWTOptionsDTO>(builder.Configuration.GetSection("JWTOptions"));

            builder.Services.AddAuthenticationService(builder.Configuration);

            var app = builder.Build();

            #region Seed Data
            await app.MigrateDatabaseAsync();
            await app.MigrateIdentityDatabaseAsync();
            await app.SeedDataAsync();
            await app.SeedIdentityDataAsync();

            #endregion

            //Exception Handler MiddleWare
            app.UseMiddleware<ExceptionHandlerMiddleWare>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            //CORs
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
