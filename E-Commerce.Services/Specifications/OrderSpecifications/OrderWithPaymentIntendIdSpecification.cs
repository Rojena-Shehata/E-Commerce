using E_Commerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications.OrderSpecifications
{
    public class OrderWithPaymentIntendIdSpecification : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntendIdSpecification(string paymentIntentId) : base(order=>order.PaymentIntentId==paymentIntentId)
        {

        }
    }
}
