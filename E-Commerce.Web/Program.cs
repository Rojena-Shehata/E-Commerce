
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Presentation.CustomeMiddleWares;
using E_Commerce.Presistence;
using E_Commerce.Presistence.Data.DataSeed;
using E_Commerce.Presistence.Data.DbContexts;
using E_Commerce.Presistence.IdentityData.DataSeed;
using E_Commerce.Presistence.IdentityData.DbContexts;
using E_Commerce.Presistence.Repositories;
using E_Commerce.Services;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

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
            builder.Services.AddCORsPolicy();

            builder.Services.AddSwaggerServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();

            builder.Services.ConfigureApiBehaviourOptions();

            builder.Services.Configure<JWTOptionsDTO>(builder.Configuration.GetSection("JWTOptions"));

            builder.Services.AddAuthenticationService(builder.Configuration);

            /////////////////////
            var app = builder.Build();

            #region Seed Data

            await app.MigrateDatabaseAsync();
            await app.MigrateIdentityDatabaseAsync();
            await app.SeedDataAsync();
            await app.SeedIdentityDataAsync();

            #endregion
            app.UseCustomExceptionMiddleWare();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();
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
