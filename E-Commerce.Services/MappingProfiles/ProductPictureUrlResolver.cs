using AutoMapper;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;
            if(source.PictureUrl.StartsWith("http")) //if Url
                return source.PictureUrl;
            var baseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            if(string.IsNullOrEmpty( baseUrl ) )
                return string.Empty;
            var imageUrl = $"{baseUrl}/{source.PictureUrl}";

            return imageUrl;

        }
    }
}
