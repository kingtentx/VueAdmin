using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VueAdmin.Core.Enums;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;
using VueAdmin.Web.Models;
using System.Threading.Tasks;

namespace VueAdmin.Web.Controllers
{

    [Authorize]
    public class AdminController : AdminBaseController
    {

        private ICacheService _cache;
        private IMapper _mapper;
        private IPermissionService _permission;
        private IRepository<Admin> _adminRepository;


        public AdminController(
             ICacheService cache,
             IMapper mapper,
             IPermissionService permission,
             IRepository<Admin> adminRepository

            )
        {

            _cache = cache;
            _mapper = mapper;
            _adminRepository = adminRepository;
            _permission = permission;


        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ReLogin()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //清空前角色缓存
            _cache.Remove(CacheKey.PermissionMenu + LoginUser.Roles);
            return Json(new ResultModel() { Code = (int)ResultCode.Success, Message = "ok" });

        }

        //[PermissionFilter(MenuCode.System_Admin, PermissionType.View)]
        public IActionResult Index()
        {
            var user = LoginUser;
            ViewBag.UserName = user.UserName;
            var model = _permission.GetLeftMenus(user);
            return View(model);
        }

        public IActionResult Main()
        {           
            ViewBag.UserName = LoginUser.UserName;

            return View();
        }

        public IActionResult Password()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdatePassword(string txtOld, string txtNew, string txtNew2)
        {
            var result = new ResultModel();

            if (string.IsNullOrEmpty(txtNew))
            {
                result.Message = "请输入新密码！";
                return Json(result);

            }
            if (string.Compare(txtNew, txtNew2) != 0)
            {
                result.Message = "两次密码输入不相同!";
                return Json(result);
            }
            if (string.Compare(txtOld, txtNew) == 0)
            {
                result.Message = "旧密码与新密码不能一样！";
                return Json(result);
            }

            var admin = _adminRepository.GetOne(LoginUser.UserId);

            if (admin != null && admin.Password == StringHelper.ToMD5(txtOld))
            {
                admin.Password = StringHelper.ToMD5(txtNew);
                if (_adminRepository.Update(admin))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Message = "修改成功";
                }
            }
            else
            {
                result.Code = (int)ResultCode.Fail;
                result.Message = "原密码输入错误！";
            }

            return Json(result);
        }
    }
}
