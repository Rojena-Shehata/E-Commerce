using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public class OrderRequestDTO
    {
        public string BasketId { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShipToAddress { get; set; } = default!;
    }


}
