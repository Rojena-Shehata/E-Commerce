
using E_Commerce.Domain.Contracts;
using E_Commerce.Presistence.Data.DataSeed;
using E_Commerce.Presistence.Data.DbContexts;
using E_Commerce.Presistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.MappingProfiles;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Web.Extensions;
using Microsoft.EntityFrameworkCore;
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
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
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

            var app = builder.Build();

            #region Seed Data
            await app.MigrateDatabaseAsync();
            await app.SeedDataAsync();
            
            #endregion

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
