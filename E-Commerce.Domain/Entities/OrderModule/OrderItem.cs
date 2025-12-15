using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.OrderModule
{
    public class OrderItem:BaseEntity<int>
    {
        public ProductItemOrdered Product { get; set; } = default!;
        public decimal Price { get; set; } //Price Per Unit
        public int Quantity { get; set; }
    }
}
