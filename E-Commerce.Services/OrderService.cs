using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specifications.OrderSpecifications;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class OrderService(IBasketRepository basketRepository,IMapper mapper,IUnitOfWork unitOfWork) : IOrderService
    {
        
        public async Task<Result<OrderResponseDTO>> CreateOrderAsync(OrderRequestDTO orderRequestDTO, string email)
        {

            if (orderRequestDTO.ShipToAddress is null)
                return Error.NotFound("Address.NotFound","You Entered Null Address");

            var customerBasket = await basketRepository.GetBasketAsync(orderRequestDTO.BasketId);
            if (customerBasket is null)
                return Error.NotFound("Basket.NotFound", $"Basked With Id:{orderRequestDTO.BasketId} IS Not Found");

            // Remove Order with the same Payment Intent Id
            if (string.IsNullOrEmpty(customerBasket.PaymentIntentId))
                return Error.NotFound("PaymentIntentId.NotFound", "PaymentIntentId is Null or Empty");

            var orderRepo = unitOfWork.GetRepository<Order, Guid>();

            var orderWithPaymentIntentIdSpec = new OrderWithPaymentIntendIdSpecification(customerBasket.PaymentIntentId);
            var existingOrder =await orderRepo.GetByIdAsync(orderWithPaymentIntentIdSpec);
            if(existingOrder is not null)
                orderRepo.Delete(existingOrder);

            //orderAddress
            var orderAddress =mapper.Map<AddressDTO,OrderAddress>(orderRequestDTO.ShipToAddress);
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in customerBasket.Items)
            {
                var product =await unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product is null)
                    return Error.NotFound("Product.NotFpund", $"Product In Basket With Id:{item.Id} Is Not Exist");
                orderItems.Add(MapOrderItemFromBasketItem(item,product));
            }

            //delivery Method
            var deliveryMethod =await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderRequestDTO.DeliveryMethodId);
            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", $"Delivery Method With Id:{orderRequestDTO.DeliveryMethodId} Is Not Found");
            //subTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            //Add Order
            var order = new Order(email, orderAddress, deliveryMethod, orderItems, subTotal, customerBasket.PaymentIntentId);
            
            orderRepo.Add(order);
           var count=await unitOfWork.SaveChangesAsync();
            if (count <= 0)
                return Error.Failure("Order.Failed", "Order Failed To Be Created");
            return mapper.Map<OrderResponseDTO>(order);

        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMeyhods=await unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            if (deliveryMeyhods is null)
                return Error.NotFound("DeliveryMethods.NotFound", "No Delivery Methods Found");

            var result= mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(deliveryMeyhods);
            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(result);
        }

        public async Task<Result<IEnumerable<OrderResponseDTO>>> GetAllOrdersForSpecificUserAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Error.NotFound("Email.NotFound", "Email Is Null Or Empty");
            var specs=new OrderSpecification(email);
            var orders =await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(specs);
            if(orders is null)
                return Error.NotFound("Orders.NotFound", "No Orders Was Found");
            var mappedOrders= mapper.Map<IEnumerable<Order>,IEnumerable<OrderResponseDTO>>(orders);
            return Result<IEnumerable<OrderResponseDTO>>.Ok(mappedOrders);


        }

        public async Task<Result<OrderResponseDTO>> GetOrderForSpecificUserAsync(Guid orderId, string email)
        {
            var specs = new OrderSpecification(orderId, email);
            var order=await unitOfWork.GetRepository<Order,Guid>().GetByIdAsync(specs);
            if (order is null)
                return Error.NotFound("Orders.NotFound", "No Orders Was Found");
            return mapper.Map<Order,OrderResponseDTO>(order);
        }

        private OrderItem MapOrderItemFromBasketItem(BasketItem item,Product product)
        {
            return new OrderItem()
            {
                Product=new ProductItemOrdered()
                {
                    ProductId= product.Id,
                    ProductName= product.Name,
                    PictureUrl= product.PictureUrl,
                },
                Price = product.Price,
                Quantity = item.Quantity
            };
        }
    }
}
