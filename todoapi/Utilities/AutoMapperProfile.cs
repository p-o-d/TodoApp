using AutoMapper;
using todoapi.Dtos;
using todoapi.Entities;

namespace todoapi.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AuthUserDto>();
        }
    }
}