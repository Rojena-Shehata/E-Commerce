using E_Commerce.Domain.Entities.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket customerBasket,TimeSpan timeToLive=default);
        Task<CustomerBasket?> GetBasketAsync(string  basketId);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
