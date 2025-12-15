using AutoMapper;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<DeliveryMethod, DeliveryMethodDTO>();

            CreateMap<AddressDTO, OrderAddress>().ReverseMap();

            //CreateMap<Order,OrderResponseDTO>()
            //    .ForMember(dest=>dest.DeliveryMethod,opt=>opt.MapFrom(src=>src.DeliveryMethod.ShortName))
            //    .ForMember(dest=>dest.Total,opt=>opt.MapFrom(src=>src.GetTotal()));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.DeliveryCost, opt => opt.MapFrom(src => src.DeliveryMethod.Cost)) 
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal())) ;
        }
    }
}
