using AutoMapper;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile() 
        {
            CreateMap<ProductBrand,BrandDTO>();

            CreateMap<ProductType,TypeDTO>();

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductType, options => options.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.ProductBrand, op => op.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());
                

        }
    }
}
