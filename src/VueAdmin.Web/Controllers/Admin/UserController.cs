using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VueAdmin.Core.Enums;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;
using VueAdmin.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VueAdmin.Web.Controllers
{
    [Authorize]
    public class UserController : AdminBaseController
    {

        private IWebHostEnvironment _hostingEnv;
        private IPermissionService _permission;
        //private WeixinUtils _weixin;
        private IMapper _mapper;
        private IRepository<User> _userRepository;
        //private IDalService<User_Reply> _replyService;

        public UserController(
            IWebHostEnvironment hostingEnv,
            IMapper mapper,
            IPermissionService permission,
            IRepository<User> userRepository
         )
        {
            _hostingEnv = hostingEnv;

            _mapper = mapper;
            _userRepository = userRepository;

            _permission = permission;
        }

        [PermissionFilter(MenuCode.Wecat_User, PermissionType.View)]
        public IActionResult Index()
        {
            ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_User, PermissionType.Edit);
            return View();
        }

        [HttpGet]
        [PermissionFilter(MenuCode.Wecat_User, PermissionType.View)]
        public async Task<JsonResult> GetList(int pageIndex = 1, int pageSize = 10)
        {
            var result = new ResultModel<List<WeChatUserModel>>();

            var name = HttpContext.Request.Query["openid"].ToString().Trim();

            var where = LambdaHelper.True<User>();

            if (!string.IsNullOrWhiteSpace(name))
                where = where.And(p => p.Openid == name);

            var query = await _userRepository.GetListAsync(where, p => p.Id, pageIndex, pageSize);

            var data = _mapper.Map(query.List, new List<WeChatUserModel>());

            result.Code = (int)ResultCode.Success;
            result.Message = "成功";
            result.Count = query.Count;
            result.Data = data;

            return Json(result);

        }

        #region 同步用户
        //[HttpGet]
        //public async Task<JsonResult> SyncUserInfo()
        //{
        //    var result = new ResultDto() ;

        //    if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_User, PermissionType.Edit))
        //    {
        //        result.Code = (int)ResultCode.Nopermit;
        //        result.Msg = "无操作权限";

        //        return Json(result);
        //    }

        //    var url = WeixinApi.GetUserList(_weixin.GetAccessToken);
        //    var data = Common.GetDownloadString(url);
        //    //log.Info(data);

        //    var list = JsonConvert.DeserializeObject<WxUsersResult>(data);

        //    string next_openid = list.NextOpenid;
        //    decimal pages = Math.Ceiling((decimal)list.Total / (decimal)list.Count); //取总页数   

        //    for (int i = 0; i < pages; i++)
        //    {
        //        next_openid = await SyncUser(i == 0 ? list : null, next_openid);
        //    }

        //    if (list.Total > 0)
        //    {
        //        result.Code = (int)ResultCode.Success;
        //        result.Message = $"更新用户数据{list.Total}条！";
        //    }

        //    return Json(result);
        //}

        ///// <summary>
        ///// 同步用户信息
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="next_openid"></param>
        ///// <param name="flag"></param>
        ///// <returns></returns>
        //private async Task<string> SyncUser(WxUsersResult list, string next_openid)
        //{
        //    var userList = list;

        //    if (list == null)
        //    {
        //        var url = WeixinApi.GetUserList(_weixin.GetAccessToken, next_openid);
        //        var data = Common.GetDownloadString(url);
        //        userList = JsonConvert.DeserializeObject<WxUsersResult>(data);
        //    }

        //    foreach (var id in userList.Data.Openid)
        //    {
        //        var wxUser = _weixin.GetUser(id);
        //        var user = _mapper.Map<User>(wxUser);
        //        user.TagidList = wxUser.TagidList.Length > 0 ? string.Join(",", wxUser.TagidList) : "";

        //        var model = _userRepository.GetModel(p => p.Openid == id).AsNoTracking().FirstOrDefault();
        //        if (model == null)
        //        {
        //            user.CreateTime = DateTime.Now;
        //            await _userRepository.AddAsync(user);
        //        }
        //        else
        //        {
        //            user.Id = model.Id;
        //            user.Telphone = model.Telphone;
        //            await _userRepository.UpdateAsync(user);
        //        }
        //    }

        //    return userList.NextOpenid;
        //}
        #endregion

    }
}
