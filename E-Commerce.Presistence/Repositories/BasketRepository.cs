using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connectionMultiplexer) 
        { 
            _database=connectionMultiplexer.GetDatabase();
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket customerBasket, TimeSpan timeToLive = default)
        {
            var jsonBasket = JsonSerializer.Serialize(customerBasket);
            var isCreatedOrUpdated=await  _database.StringSetAsync(customerBasket.Id, jsonBasket
                                                      , (timeToLive==default)?TimeSpan.FromDays(7):timeToLive);
            if (isCreatedOrUpdated)
            {
                return await GetBasketAsync(customerBasket.Id);
            }
            else
                return null;
            
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
           var basket=await _database.StringGetAsync(basketId);
            if(basket.IsNullOrEmpty)
                return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }
    }
}
