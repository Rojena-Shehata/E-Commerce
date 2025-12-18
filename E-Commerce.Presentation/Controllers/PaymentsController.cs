using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IServiceManager _paymentService;

        public PaymentsController(IServiceManager serviceManager)
        {
            _paymentService = serviceManager;
        }
        
        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<ActionResult<BasketDTO>> CreateORUpdatePaymentIntent02(string basketId)
        {
            var basket =await _paymentService.PaymentService.CreateORUpdatePaymentIntentAsync(basketId);
            return HandleResult(basket);
        }
        //stripe listen --forward-to https://localhost:7047/api/Payments/webhook

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
                  var signatureHeader = Request.Headers["Stripe-Signature"];
                await _paymentService.PaymentService.UpdateOrderPaymentStatus(json, signatureHeader);
                
                return new EmptyResult();
                               
        }

    }
}
