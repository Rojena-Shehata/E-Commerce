using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.ProductDTOs
{
    public class ProductQueryParameters
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        
    }
}
