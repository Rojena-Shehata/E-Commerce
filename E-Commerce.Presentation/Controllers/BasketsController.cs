using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class BasketsController: ApiBaseController
    {
        private readonly IServiceManager _serviceManager;

        public BasketsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet]
        public async Task<ActionResult<BasketDTO> >GetBasket(string id)
        {
            var basket=await _serviceManager.BasketService.GetBasketAsync(id);
            return HandleResult<BasketDTO>(basket);
        }
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basketInput)
        {
            var basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basketInput);
            return Ok(basket);

        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var basket = await _serviceManager.BasketService.DeleteBasketAsync(id);
            return Ok(basket);

        }


    }
}
