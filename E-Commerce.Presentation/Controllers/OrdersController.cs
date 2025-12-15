using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.BasketDTOs;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class OrdersController:ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        ////}
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderRequestDTO orderRequestDTO)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _orderService.CreateOrderAsync(orderRequestDTO, email);
            return HandleResult<OrderResponseDTO>(result);
        }
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderForSpecificUserById(Guid id)
        {
            var emaail = GetUserEmail();
            var result =await _orderService.GetOrderForSpecificUserAsync(id, emaail);
            return HandleResult<OrderResponseDTO>(result);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrderForSpecificUserById()
        {
            var emaail = GetUserEmail();
            var result =await _orderService.GetAllOrdersForSpecificUserAsync(emaail);
            return HandleResult(result);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethods()
        {
            var result=await _orderService.GetAllDeliveryMethodsAsync();
            return HandleResult<IEnumerable<DeliveryMethodDTO>>(result);
        }

        //[HttpPost]
        //public async Task<ActionResult<BasketDTO>> GetBasket(OrderRequestDTO id)
        //{
        //    var basket = new BasketDTO("h", []);
        //    return HandleResult<BasketDTO>(basket);
        //}
        private string? GetUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);

        }

    }
}
