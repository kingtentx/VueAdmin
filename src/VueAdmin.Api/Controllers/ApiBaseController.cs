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
                        Role = !string.IsNullOrWhiteSpace(identity.FindFirst(ClaimTypes.Role)?.Value) ? StringHelper.StrArrToIntArr(identity.FindFirst(ClaimTypes.Role)?.Value.ToString().Split(',')) : new int[] { 0 }
                    };
                    return user;
                }
                catch
                {
                    return new LoginUserDto();
                }
            }
        }

        ///// <summary>
        ///// 检查IP
        ///// </summary>
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public bool CheckIPAddress(IConfiguration config)
        //{
        //    var isCheckIp = Convert.ToBoolean(config["WhiteIP:IsEnabled"]);
        //    if (!isCheckIp)
        //        return true;

        //    try
        //    {
        //        var ip = GetIPAddress();
        //        Log.Information("当前请求IP-->" + ip);

        //        var arr1 = ip.Split(',');
        //        var arr2 = config["WhiteIP:Address"].Split(',');
        //        var intersect = arr1.Intersect(arr2);
        //        if (intersect.Any())
        //            return true;

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("获取IP地址异常-->" + ex.Message);
        //        return false;
        //    }
        //}

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
