using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Presistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            try
            {


                var hasTypes = await _dbContext.ProductTypes.AnyAsync();
                var hasBrands = await _dbContext.ProductBrands.AnyAsync();
                var hasProducts = await _dbContext.Products.AnyAsync();
                if (hasTypes && hasBrands && hasProducts)
                    return;

                if (!hasBrands)
                {
                  await  SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                }
                if (!hasTypes)
                {
                  await  SeedDataFromJsonAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
                }
               await _dbContext.SaveChangesAsync();
                if(!hasProducts)
                {
                   await SeedDataFromJsonAsync<Product, int>("products.json",_dbContext.Products);
                }
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex) 
            {
                    Console.WriteLine($"Data Seeding Failed {ex}");
            }
        }

        private async Task SeedDataFromJsonAsync<T,TKey>(string fileName,DbSet<T> dbset)where T:BaseEntity<TKey>
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new FileNotFoundException($"Null or Empty FileName argument {fileName}");
            //E:\Course--\Route\06)API\E - CommerceSolution\E - Commerce.Presistence\Data\DataSeed\JsonFiles
            var filePath = @"../E-Commerce.Presistence/Data/DataSeed/JsonFiles/"+fileName;
            if(!File.Exists(filePath) )
                throw new FileNotFoundException($"File Not  Found => {fileName} ,FilePath =>{filePath}");

            try
            {
               using var fileStream = File.OpenRead(filePath);
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive=true
                };
                var data=await JsonSerializer.DeserializeAsync<List<T>>(fileStream,options);
                if(data is not null)
                {
                    dbset.AddRange(data);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error while Read json File : {ex}");
            }


        }
    }
}
