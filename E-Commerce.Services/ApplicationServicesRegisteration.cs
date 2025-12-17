using E_Commerce.ServicesAbstraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();
            services.AddScoped<ICacheService, CacheService>();

            services.AddScoped<IproductService, ProductService>();
            services.AddScoped<Func<IproductService>>(serviceProvider => 
                                                                       () => serviceProvider.GetRequiredService<IproductService>());

            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<Func<IBasketService>>(serviceProvider =>
                                                                      () => serviceProvider.GetRequiredService<IBasketService>());

            services.AddScoped<IPaymentService,PaymentService>();
            services.AddScoped<Func<IPaymentService>>(serviceProvider => 
                                                                       () => serviceProvider.GetRequiredService<IPaymentService>());

            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<Func<IOrderService>>(serviceProvider => 
                                                                      () => serviceProvider.GetRequiredService<IOrderService>());

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<Func<IAuthenticationService>>(serviceProvider=>
                                                                             ()=> serviceProvider.GetRequiredService<IAuthenticationService>());




            //AutoMapper
            //-version 14
            services.AddAutoMapper(typeof(ServicesAssemblyReference).Assembly);
            #region Automapper version 15
            //-In AutoMapper version 15, It doesn't work in production without licenseKey, but work in development only
            ////builder.Services.AddAutoMapper(x => x.AddProfile<ProductProfile>());
            ////builder.Services.AddTransient<ProductPictureUrlResolver>();
            //
            //-version 15 with assembly +License key+ without license key work in development only
            //// builder.Services.AddAutoMapper(x => x.LicenseKey="",typeof(ProductProfile).Assembly);

            #endregion

            return services;
        }
    }
}
