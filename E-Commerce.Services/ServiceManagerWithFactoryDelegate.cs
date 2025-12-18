using E_Commerce.ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class ServiceManagerWithFactoryDelegate(Func<IproductService> productFactory,
                                                   Func<IBasketService> basketFactory,
                                                   Func<IOrderService> orderFactory,
                                                   Func<IPaymentService>paymentFactory,
                                                   Func<IAuthenticationService> authenticanFactory
                                                    ) : IServiceManager
    {
        public IproductService ProductService => productFactory.Invoke();

        public IBasketService BasketService => basketFactory.Invoke();

        public IOrderService OrderService => orderFactory.Invoke();

        public IPaymentService PaymentService => paymentFactory.Invoke();

        public IAuthenticationService AuthenticationService => authenticanFactory.Invoke();
    }
}
