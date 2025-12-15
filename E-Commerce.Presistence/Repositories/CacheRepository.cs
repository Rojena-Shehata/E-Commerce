using E_Commerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _database=connectionMultiplexer.GetDatabase();
        }

        public async Task<string?> GetAsync(string cacheKey)
        {
          return await  _database.StringGetAsync(cacheKey);
        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan timeToLive)
        {
          await  _database.StringSetAsync(cacheKey, cacheValue, timeToLive);
        }
    }
}
