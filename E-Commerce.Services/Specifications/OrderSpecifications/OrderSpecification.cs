using E_Commerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications.OrderSpecifications
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(Guid id, string email) : base(CreateCriteria(email,id))
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);
        }
        public OrderSpecification(string email) : base(CreateCriteria(email))
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);

            AddOrderByDesc(x=>x.OrderDate);
        }


        private static Expression<Func<Order, bool>>? CreateCriteria(string email, Guid? Id=null)
        {
            return x => (string.IsNullOrEmpty(email) || x.BuyerEmail == email) &&
                        (!Id.HasValue || x.Id == Id);
        }

    }
}
