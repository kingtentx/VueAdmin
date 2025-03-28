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

            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<User, CreateUpdateUserDto>().ReverseMap();

            CreateMap<User, LoginUserDto>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(c => c.Id))
                .ReverseMap();
        }
    }
}
