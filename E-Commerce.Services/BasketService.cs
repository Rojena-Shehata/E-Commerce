using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Services.Exceptions.NotFoundExceptions;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
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

        public async Task<Result<BasketDTO>> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var customerBasket = _mapper.Map<BasketDTO, CustomerBasket>(basket);
          var createdOrUpdatedBasket= await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);
            if (createdOrUpdatedBasket is null)
                return Error.Failure("Create Or Update Basket Failure", "Fail to Create or Update new Basket");
            return _mapper.Map<CustomerBasket, BasketDTO>(createdOrUpdatedBasket);

        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }

        public async Task<Result<BasketDTO>> GetBasketAsync(string basketId)
        {
           var customerBasket= await _basketRepository.GetBasketAsync(basketId);
            if (customerBasket is null)
                return Error.NotFound("Basket.NotFound", $"Basked With Id:{basketId} IS Not Found");
            return  _mapper.Map<CustomerBasket,BasketDTO>(customerBasket);
        }
    }
}
