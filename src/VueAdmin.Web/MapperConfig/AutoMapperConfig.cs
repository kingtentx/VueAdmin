using AutoMapper;
using VueAdmin.Data;
using VueAdmin.Web.Areas.Dtos;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VueAdmin.Web.Models.MapperConfig
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //admin Model
            #region admin

            CreateMap<Admin, LoginAdminModel>().ReverseMap();
            CreateMap<Menu, MenuModel>().ReverseMap();       
            CreateMap<PictureGallery, PictureGalleryModel>().ReverseMap();
            CreateMap<Attachments, AttachmentsModel>().ReverseMap();
               
            //CreateMap<Navigation, NavigationModel>()
            //    .ForMember(dest => dest.Id, opts => opts.MapFrom(c => c.NavigationId))
            //    .ForMember(dest => dest.Pid, opts => opts.MapFrom(c => c.ParentId))
            //    .ReverseMap();
            #endregion


            //api Dto
            #region api
            CreateMap<Admin, LoginUserDto>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(c => c.AdminId))
                .ReverseMap();

      
            #endregion
        }
    }
}
