using AutoMapper;
using VueAdmin.Api.Dtos;
using VueAdmin.Data;
using Newtonsoft.Json;
using VueAdmin.Api.Dtos.User;

namespace VueAdmin.Api.Models.MapperConfig
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {

            CreateMap<User, UserDto>()
                .ForMember(d => d.Phone, m => m.MapFrom(c => c.Telphone))
                .ForMember(d => d.Status, m => m.MapFrom(c => c.IsActive)).ReverseMap();

            CreateMap<User, CreateUpdateUserDto>()              
                .ForMember(d => d.Phone, m => m.MapFrom(c => c.Telphone))
                .ForMember(d => d.Status, m => m.MapFrom(c => c.IsActive))
                .ReverseMap();

            CreateMap<User, LoginUserDto>()
                .ForMember(d => d.UserId, m => m.MapFrom(c => c.Id))
                .ReverseMap();
        }
    }
}
