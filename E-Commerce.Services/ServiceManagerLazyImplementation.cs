using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class ServiceManagerLazyImplementation(IUnitOfWork unitOfWork,IMapper mapper,
                                                    IBasketRepository basketRepository,IConfiguration configuration,
                                                    UserManager<ApplicationUser> userManager,IOptions<JWTOptionsDTO> jwtOptions) : IServiceManager
    {
        private readonly Lazy<IproductService> _lazyProductService= new Lazy<IproductService>(() => new ProductService(unitOfWork,mapper));
        private readonly Lazy<IBasketService> _lazyBasketService = new Lazy<IBasketService>(()=>new BasketService(basketRepository,mapper));
        private readonly Lazy<IOrderService> _lazyOrderService= new Lazy<IOrderService>(()=>new OrderService(basketRepository,mapper,unitOfWork));
        private readonly Lazy<IPaymentService> _lazyPaymentService = new Lazy<IPaymentService>(()=>new PaymentService(basketRepository,unitOfWork,mapper,configuration));
        private readonly Lazy<IAuthenticationService> _lazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, jwtOptions, mapper));
        public IproductService ProductService => _lazyProductService.Value ;

        public IBasketService BasketService => _lazyBasketService.Value;

        public IOrderService OrderService =>_lazyOrderService.Value ;

        public IPaymentService PaymentService => _lazyPaymentService.Value;

        public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;

    }
}
