using AutoMapper;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            var pictureUrl = source.Product.PictureUrl;
            if (string.IsNullOrEmpty(pictureUrl))
                return string.Empty;
            if(pictureUrl.ToLower().StartsWith("http"))
                return pictureUrl;
            var baseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            if(string.IsNullOrEmpty(baseUrl)) 
                return string.Empty;
            return $"{baseUrl}/{pictureUrl}";
        }
    }
}
