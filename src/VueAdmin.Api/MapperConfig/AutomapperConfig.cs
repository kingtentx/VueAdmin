using AutoMapper;
using VueAdmin.Api.Dtos;
using VueAdmin.Data;


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

            CreateMap<PictureGallery, PictureGalleryDto>().ReverseMap();
            CreateMap<Attachments, AttachmentsDto>().ReverseMap();

            CreateMap<Menu, CreateUpdateMenuDto>().ReverseMap();
            CreateMap<Menu, MenuDto>().ReverseMap();
            CreateMap<Menu, MenuItemDto>().ReverseMap();          

            CreateMap<Role, CreateUpdateRoleDto>().ForMember(d => d.Status, m => m.MapFrom(c => c.IsActive)).ReverseMap();
            CreateMap<Role, RoleDto>().ForMember(d => d.Status, m => m.MapFrom(c => c.IsActive)).ReverseMap();
            CreateMap<Role, RoleItemDto>().ReverseMap();

        }
    }
}
