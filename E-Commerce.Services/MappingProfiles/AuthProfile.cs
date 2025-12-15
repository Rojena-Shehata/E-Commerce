using AutoMapper;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Address,AddressDTO>().ReverseMap();
        }
    }
}
