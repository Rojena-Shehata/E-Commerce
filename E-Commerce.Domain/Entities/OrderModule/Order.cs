using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.OrderModule
{
    public class Order:BaseEntity<Guid>
    {
        public Order()
        {
        }

        public Order(string buyerEmail, OrderAddress shipToAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }


        public string BuyerEmail { get; set; } = default!;
        public OrderAddress ShipToAddress { get; set; } = default!;//Owned
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public ICollection<OrderItem> Items { get; set; } = [];
        public decimal SubTotal { get; set; }//Total  Price Of Items
        public string PaymentIntentId { get; set; } = default!;

        public OrderStatus Status { get; set; }=OrderStatus.Pending;
        public DateTimeOffset OrderDate {  get; set; }= DateTimeOffset.Now;
        public int DeliveryMethodId { get; set; }//FK
        public decimal GetTotal() => SubTotal+DeliveryMethod.Cost;


    }
}
