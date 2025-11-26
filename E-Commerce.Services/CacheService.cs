using E_Commerce.Domain.Contracts;
using E_Commerce.ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task<string?> GetAsync(string cacheKey)
        {
            return await _cacheRepository.GetAsync(cacheKey);
        }

        public async Task SetAsync(string cacheKey, object value, TimeSpan timeToLive)
        {
            var jsonValue=JsonSerializer.Serialize(value,new JsonSerializerOptions()
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            });
           await _cacheRepository.SetAsync(cacheKey, jsonValue, timeToLive);
        }
    }
}
