using Microsoft.AspNetCore.Mvc;
using VueAdmin.Web.Models;
using System;
using System.Security.Claims;

namespace VueAdmin.Web.Controllers
{

    public class AdminBaseController : Controller
    {
        /// <summary>
        /// 后台登录用户
        /// </summary>   
        /// <returns></returns>      
        public LoginAdminModel LoginUser
        {
            get
            {
                var identity = (ClaimsIdentity)HttpContext.User.Identity;

                LoginAdminModel user = new LoginAdminModel()
                {
                    UserId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Sid)?.Value),
                    UserName = identity.FindFirst(ClaimTypes.Name)?.Value,
                    Roles = identity.FindFirst(ClaimTypes.Role)?.Value,                 
                    IsAdmin = Convert.ToBoolean(identity.FindFirst(ClaimTypes.System)?.Value)
                };

                return user;
            }
        }
    }
}
