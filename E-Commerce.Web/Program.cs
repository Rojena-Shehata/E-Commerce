
using E_Commerce.Domain.Contracts;
using E_Commerce.Presentation.CustomeMiddleWares;
using E_Commerce.Presistence.Data.DataSeed;
using E_Commerce.Presistence.Data.DbContexts;
using E_Commerce.Presistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.MappingProfiles;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //DbContext
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // DataSedding 
            builder.Services.AddScoped<IDataInitializer,DataInitializer>();

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

            //validation options Handling
            //builder.Services.Configure<ApiBehaviorOptions>(config =>
            //{
            //    config.InvalidModelStateResponseFactory=actionContext =>
            //    {
            //        var errors=actionContext.ModelState.Where(M=>M.Value.Errors.Any())
            //                                .ToDictionary(x=>x.Key,x=>x.Value.Errors
            //                                                          .Select(x=>x.ErrorMessage).ToList());
            //        var problem = new ProblemDetails()
            //        {
            //            Title= "Validation Error !!",
            //            Status=StatusCodes.Status400BadRequest,
            //            Detail= "One or more validation errors occurred.",
            //            Extensions = { { "Errors",errors} }
            //        };
            //        return new BadRequestObjectResult(problem);
            //    };
            //});

            builder.Services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
                
            });
            var app = builder.Build();

            #region Seed Data
            await app.MigrateDatabaseAsync();
            await app.SeedDataAsync();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
