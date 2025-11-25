using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.BasketDTOs
{
    public record BasketDTO
        (
        string Id,
        ICollection<BasketItemDTO>Items
        );
    
    
}
