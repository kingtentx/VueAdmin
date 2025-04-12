using Microsoft.AspNetCore.Mvc;
using VueAdmin.Api.Dtos;
using Serilog;
using System.Security.Claims;
using VueAdmin.Helper;

namespace VueAdmin.Api.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        /// <summary>
        /// 登录用户
        /// </summary>   
        /// <returns></returns>      
        public LoginUserDto LoginUser
        {
            get
            {
                try
                {
                    var identity = (ClaimsIdentity)HttpContext.User.Identity;
                    LoginUserDto user = new LoginUserDto()
                    {
                        UserId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Sid)?.Value),
                        UserName = identity.FindFirst(ClaimTypes.Name)?.Value,
                        Roles = !string.IsNullOrWhiteSpace(identity.FindFirst(ClaimTypes.Role)?.Value) ? StringHelper.StrArrToIntArr(identity.FindFirst(ClaimTypes.Role)?.Value.ToString().Split(',')) : new int[] { 0 },
                        IsAdmin= Convert.ToBoolean(identity.FindFirst(ClaimTypes.System)?.Value)
                    };
                    return user;
                }
                catch
                {
                    return new LoginUserDto();
                }
            }
        }


        /// <summary>
        /// 获取IP
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetIPAddress()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var ip = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();//X-Forwarded-For可能会包含多个IP
            if (string.IsNullOrEmpty(ip))
            {
                return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            else
            {
                return ip.IndexOf(',') > 0 ? ip.Split(',')[0] : ip;
            }
        }


    }
}
