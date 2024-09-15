using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueAdmin.Core;
using VueAdmin.Core.Enums;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;
using VueAdmin.Web.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VueAdmin.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        private ICacheService _cache;     
        private IRepository<Admin> _adminRepository;
        private IRepository<AdminLogin> _logRepository;
        private IRepository<Role> _roleRepository;


        public AuthorizeController(ICacheService cache,            
             IRepository<Admin> adminRepository,
             IRepository<AdminLogin> loginRepository,
             IRepository<Role> roleRepository)
        {
            _adminRepository = adminRepository;
            _logRepository = loginRepository;
            _cache = cache;          
            _roleRepository = roleRepository;
        }

        #region 登录
        /// <summary>
        /// asp.net cookies 登录
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> LoginPost(AdminInputModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var code = _cache.Get(CacheKey.ValidateCode + viewModel.ValidateKey) ?? "";
                if (viewModel.ValidateCode.ToLower() != code.ToString().ToLower() || string.IsNullOrEmpty(viewModel.ValidateCode))
                {
                    return BadRequest("验证码错误");
                }
                if (!string.IsNullOrEmpty(viewModel.UserName) && !string.IsNullOrEmpty(viewModel.Password))//判断账号密码是否正确
                {
                    var admin = _adminRepository.GetOne(p => p.UserName == viewModel.UserName && p.Password == StringHelper.ToMD5(viewModel.Password));
                    if (admin == null)
                        return BadRequest("用户名密码错误");

                    if (!admin.IsActive)
                        return BadRequest("帐户已被禁用");
                   
                    //创建用户身份标识
                    var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaims(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Sid,admin.AdminId.ToString()),
                        new Claim(ClaimTypes.Name,admin.UserName), 
                        new Claim(ClaimTypes.Role,!string.IsNullOrWhiteSpace(admin.Roles)?admin.Roles:"0"),
                        new Claim(ClaimTypes.System,admin.IsAdmin.ToString())
                    });

                    //登录用户，相当于ASP.NET中的FormsAuthentication.SetAuthCookie
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity)
                        , new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddHours(CacheKey.ExpirationTimeLen_5),
                            // 持久保存
                            IsPersistent = true,
                            //AllowRefresh为true，表示如果用户登录后在超过50%的ExpiresUtc时间间隔内又访问了站点，就延长用户的登录时间
                            AllowRefresh = true
                        });

                    //var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();

                    var model = new AdminLogin()
                    {
                        UserName = admin.UserName,
                        Client = Request.Headers["User-Agent"].ToString(),
                        LoginDate = DateTime.Now,
                        LoginIp = Common.GetIPAddress()
                };

                    await _logRepository.AddAsync(model);

                    return Json(new ResultModel() { Code = (int)ResultCode.Success, Message = "ok" });
                }
            }

            return BadRequest("帐号或者密码错误");
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetImg(string key, int width = 130, int height = 38, int fontsize = 24)
        {
            string code = string.Empty;
            byte[] bytes = ValidateCode.CreateValidateGraphic(out code, 4, width, height, fontsize);

            _cache.Add(CacheKey.ValidateCode + key, code, TimeSpan.FromMinutes(5));
            return File(bytes, @"image/jpeg");
        }
        #endregion

    }

}