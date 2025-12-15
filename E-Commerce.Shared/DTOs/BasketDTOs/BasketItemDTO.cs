using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.BasketDTOs
{
    public record BasketItemDTO
        (
            int Id,
            string ProductName,
            string PictureUrl,
            [Range(1,int.MaxValue,ErrorMessage ="Price must be greater than 0")]
            decimal Price,
            [Range(1,int.MaxValue,ErrorMessage ="Price must be greater than 0")]
            int Quantity
        );
    
    
}
