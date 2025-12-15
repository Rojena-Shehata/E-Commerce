using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.ServicesAbstraction
{
    public interface IOrderService
    {
        Task<Result<OrderResponseDTO>> CreateOrderAsync(OrderRequestDTO orderRequestDTO,string email);

        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodsAsync();

        Task<Result<OrderResponseDTO>> GetOrderForSpecificUserAsync(Guid orderId,string email);
        Task<Result<IEnumerable<OrderResponseDTO>>> GetAllOrdersForSpecificUserAsync(string email);
    }
}
