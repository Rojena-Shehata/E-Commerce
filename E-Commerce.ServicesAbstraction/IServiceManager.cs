using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.ServicesAbstraction
{
    public interface IServiceManager
    {
        public IproductService ProductService { get; }
        public IBasketService BasketService { get; }
        public IOrderService OrderService { get; }
        public IPaymentService PaymentService { get; }
        public IAuthenticationService AuthenticationService { get; }

    }
}
