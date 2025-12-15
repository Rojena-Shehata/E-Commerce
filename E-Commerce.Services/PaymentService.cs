using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specifications.OrderSpecifications;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.BasketDTOs;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = E_Commerce.Domain.Entities.ProductModule.Product;

namespace E_Commerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IMapper mapper,IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Result<BasketDTO>> CreateORUpdatePaymentIntentAsync(string BasketId)
        {
            //get basket
            if (string.IsNullOrEmpty(BasketId))
                return Error.NotFound("Basket.NotFound", "Basket Id is Null or Empty");
            var customerBasket=await _basketRepository.GetBasketAsync(BasketId);
            if(customerBasket is null)
                return Error.NotFound("Basket.NotFound", $"Basket with Id:{BasketId} Was Not Found");
            
            //Cechk Delivery Method.
            if (!customerBasket.DeliveryMethodId.HasValue)
                return Error.NotFound("DeliveryMethod.NotFound", "You Entered Null DeliverMethodId");
            var deliveryMethod=await _unitOfWork.GetRepository<DeliveryMethod,int>().GetByIdAsync(customerBasket.DeliveryMethodId.Value);
            if(deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", $"Delivery Method with Id:{customerBasket.DeliveryMethodId.Value} Was Not Found");
            customerBasket.ShippingPrice = deliveryMethod.Cost;

            //get Basket Item Price 
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in customerBasket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                if (product is null)
                    return Error.NotFound("Product.NotFound", $"Product With Id:{item.Id} Was Not Found");
                item.Price=product.Price;
            }
            //calculate Amount
            var basketAmount = customerBasket.Items.Sum(item => item.Quantity*item.Price) + customerBasket.ShippingPrice;

            //stripe Configuration.ApiKey
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            //Create Payment Intent [Create - Update]
            var paymentService = new PaymentIntentService();
            if (string.IsNullOrEmpty(customerBasket.PaymentIntentId)) //Create
            {
                var paymentIntentOptions = new PaymentIntentCreateOptions
                {
                    Amount= (long)basketAmount*100,
                    Currency="USD",
                    PaymentMethodTypes=["card"]
                };
              var paymentIntent= await paymentService.CreateAsync(paymentIntentOptions);
                customerBasket.PaymentIntentId = paymentIntent.Id;
                customerBasket.ClientSecret = paymentIntent.ClientSecret;
            }
            else  // Update
            {
                var paymentIntentOptions = new PaymentIntentUpdateOptions
                {
                    Amount= (long)basketAmount*100,
                };
                await paymentService.UpdateAsync(customerBasket.PaymentIntentId, paymentIntentOptions);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);

            return _mapper.Map<BasketDTO>(customerBasket);
        }


        public async Task UpdateOrderPaymentStatus(string requestBody, string requestSignatureHeader)
        {
            var stripeEvent = EventUtility.ParseEvent(requestBody);
            var endpointSecret = _configuration["StripeSettings:EndpointSecret"];

            stripeEvent = EventUtility.ConstructEvent(requestBody,
                                    requestSignatureHeader, endpointSecret);
            var paymentIntent=stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent is null)
                return;
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntendIdSpecification(paymentIntent.Id));
            if (order is null)
                return;
            // If on SDK version < 46, use class Events instead of EventTypes
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                //Update Order Status to Succeed
                
                order.Status = OrderStatus.PaymentRecieved;
                orderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                //Update Order Status to Failed
                order.Status= OrderStatus.PaymentFailed;
                orderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
        }
    }
}
