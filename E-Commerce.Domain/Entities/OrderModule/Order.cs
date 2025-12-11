using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.OrderModule
{
    public class Order:BaseEntity<Guid>
    {
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate {  get; set; }= DateTimeOffset.Now;
        public OrderStatus Status { get; set; }=OrderStatus.Pending;
        public OrderAddress ShipToAddress { get; set; } = default!;//Owned
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public ICollection<OrderItem> Items { get; set; } = [];

        public decimal SubTotal { get; set; }//Total  Price Of Items

        public decimal GetTotal() => SubTotal+DeliveryMethod.Cost;


    }
}
