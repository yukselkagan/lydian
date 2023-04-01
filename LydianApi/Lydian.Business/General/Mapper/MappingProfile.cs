using AutoMapper;
using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Business.General.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductAddDto>().ReverseMap().ForMember(x => x.Image, t => t.MapFrom(y => y.ImagePath) );
        }
    }
}
