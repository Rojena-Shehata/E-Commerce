using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.ProductModule
{
    public class Product:BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }

        public int BrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }= default!;
        public int TypeId { get; set; }
        public ProductType ProductType { get; set; }=default!;



    }
}
