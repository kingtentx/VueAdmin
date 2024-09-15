using AutoMapper;
using VueAdmin.Api.Dtos;
using VueAdmin.Data;
using Newtonsoft.Json;

namespace VueAdmin.Api.Models.MapperConfig
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            #region api
            CreateMap<Admin, LoginUserDto>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(c => c.AdminId))
                .ReverseMap();



           
            #endregion
        }
    }
}
