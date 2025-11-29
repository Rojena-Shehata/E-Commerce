using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Services.Exceptions.NotFoundExceptions;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.BasketDTOs;

namespace E_Commerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var customerBasket = _mapper.Map<BasketDTO, CustomerBasket>(basket);
          var createdOrUpdatedBasket= await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);
            
            return _mapper.Map<CustomerBasket, BasketDTO>(createdOrUpdatedBasket);

        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }

        public async Task<BasketDTO> GetBasketAsync(string basketId)
        {
           var customerBasket= await _basketRepository.GetBasketAsync(basketId);
            if (customerBasket is null)
                throw new BasketNotFoundException(basketId);
            return  _mapper.Map<CustomerBasket,BasketDTO>(customerBasket);
        }
    }
}
