using AutoMapper;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class BasketProfile:Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDTO>().ReverseMap();// <=>
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();// <=>
        }
    }
}
