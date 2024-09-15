using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VueAdmin.Web.Areas.Dtos;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;

namespace VueAdmin.Web.Areas.Controllers
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
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                LoginUserDto user = new LoginUserDto()
                {
                    UserId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Sid)?.Value),
                    UserName = identity.FindFirst(ClaimTypes.Name)?.Value
                };

                return user;
            }
        }

        /// <summary>
        /// 检查IP
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool CheckIPAddress(IConfiguration config)
        {
            var isCheckIp = Convert.ToBoolean(config["WhiteIP:IsEnabled"]);
            if (!isCheckIp)
                return true;

            try
            {
                var ip = Common.GetIPAddress();
                Log.Information("当前请求IP-->" + ip);

                var arr1 = ip.Split(',');
                var arr2 = config["WhiteIP:Address"].Split(',');
                var intersect = arr1.Intersect(arr2);
                if (intersect.Any())
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Log.Error("获取IP地址异常-->" + ex.Message);
                return false;
            }
        }


    }
}
