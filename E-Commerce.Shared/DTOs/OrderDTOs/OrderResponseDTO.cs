using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record OrderResponseDTO
    {
       public Guid Id { get; init; } = default!;
        public string BuyerEmail { get; init; } = default!;
        public ICollection<OrderItemDTO> Items {  get; init; }
        public AddressDTO ShipToAddress {  get; init; }= default!;
        public string DeliveryMethod { get; init; } = default!;
        public decimal DeliveryCost {  get; init; }
        public string Status { get; init; } = default!;
        public DateTimeOffset OrderDate {  get; init; }
        public decimal SubTotal {  get; init; }
        public decimal Total {  get; init; }
        public string PaymentIntentId { get; init; } = default!;

    };
    
}
