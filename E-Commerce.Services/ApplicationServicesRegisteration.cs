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
            services.AddScoped<IServiceManager, ServiceManagerLazyImplementation>();
            services.AddScoped<ICacheService, CacheService>();

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
