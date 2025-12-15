using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record DeliveryMethodDTO
    (
        int Id,
        string ShortName,
        string Description,
        string DeliveryTime,
        decimal Cost
     );
    
}
