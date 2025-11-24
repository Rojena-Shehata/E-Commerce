using E_Commerce.Domain.Contracts;
using E_Commerce.Presistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
          await using var scope=app.Services.CreateAsyncScope();
            var dbContextService=scope.ServiceProvider.GetRequiredService<StoreDbContext>();

             var pendingMigrations =await dbContextService.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                   await dbContextService.Database.MigrateAsync();
            return app;
        }

        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
           await using var scope= app.Services.CreateAsyncScope();
            var dataInitializerService = scope.ServiceProvider.GetRequiredService<IDataInitializer>();

           await dataInitializerService.InitializeAsync();

            return app;
        }
    }
}
