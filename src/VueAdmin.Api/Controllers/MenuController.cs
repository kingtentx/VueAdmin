using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;
using System.Reflection;
using VueAdmin.Api.Dtos;
using VueAdmin.Api.Permissions;
using VueAdmin.Core.Enums;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;

namespace VueAdmin.Api.Controllers
{
    [Route("api/menu")]
    [ApiController]
    [Authorize]
    public class MenuController : ApiBaseController
    {
        private IMapper _mapper;
        private IPermissionService _permissionService;
        private IRepository<Menu> _menuRepository;
        private IRepository<RoleMenu> _rolemenuRepository;

        public MenuController(
            IMapper mapper,
             IPermissionService permissionService,
            IRepository<Menu> menuRepository,
            IRepository<RoleMenu> rolemenuRepository
           )
        {
            _mapper = mapper;
            _menuRepository = menuRepository;
            _rolemenuRepository = rolemenuRepository;
            _permissionService = permissionService;
        }

        //[HttpGet]
        //[Route("init-menu")]      
        //public ResultDto<bool> InitMenu()
        //{
        //    var result = new ResultDto<bool>();
        //    // 读取 JSON 文件的路径
        //    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/menu_list.json");

        //    // 检查文件是否存在
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        result.Msg = "JSON 文件未找到。";
        //        return result;
        //    }
        //    string jsonContent = System.IO.File.ReadAllText(filePath);

        //    var list = JsonConvert.DeserializeObject<List<Menu>>(jsonContent);           
        //    result.Ok();
        //    return result;
        //}


        /// <summary>
        /// 获取用户信息
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        [Route("get-menus")]
        public async Task<ResultDto<List<MenuTreeDto>>> GetMenus()
        {
            var result = new ResultDto<List<MenuTreeDto>>();
            var menus = new List<Menu>();

            if (LoginUser.IsAdmin)
            {
                menus = await _menuRepository.GetListAsync(p => p.ShowLink && p.IsDelete == false);
            }
            else
            {
                var menuIds = await _rolemenuRepository.GetQueryable(p => LoginUser.Roles.Contains(p.RoleId)).Select(p => p.MenuId).ToListAsync();
                menus = await _menuRepository.GetListAsync(p => menuIds.Contains(p.Id) && p.ShowLink && p.IsDelete == false);
            }

            var root = menus.Where(p => p.ParentId == 0).OrderBy(p => p.Sort).ToList();
            var data = LoadMenusTree(root, menus);
            result.SetData(data);

            return result;
        }

        private List<MenuTreeDto> LoadMenusTree(List<Menu> roots, List<Menu> menus)
        {
            var result = new List<MenuTreeDto>();
            foreach (var root in roots.Where(p => p.MenuType < (int)MenuType.Button))
            {
                var menu = new MenuTreeDto
                {
                    Name = root.Name?.Trim(),
                    Component = root.Component,
                    Path = root.Path?.Trim(),
                    Meta = new MetaDto()
                    {
                        Title = root.Title?.Trim(),
                        Icon = root.Icon,
                        Sort = root.Sort == 0 ? 2 : root.Sort,
                        //Roles = new List<string>() { "admin", "common" },
                        Auths = menus.Where(p => p.ParentId == root.Id && p.MenuType == (int)MenuType.Button).Select(p => p.Auths).ToList(),
                    }
                };

                if (menus.Where(_ => _.ParentId == root.Id).Any())
                {
                    menu.Children = LoadMenusTree(menus.Where(p => p.ParentId == root.Id).OrderBy(p => p.Sort).ToList(), menus);
                }
                else
                {
                    menu.Children = new List<MenuTreeDto>();
                }

                result.Add(menu);
            }
            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<ResultDto<List<MenuDto>>> GetMenuList()
        {
            var result = new ResultDto<List<MenuDto>>();
            var list = await _menuRepository.GetListAsync(p => p.IsDelete == false, p => p.Sort, isAsc: true);
            var data = _mapper.Map<List<MenuDto>>(list);
            result.SetData(data);
            return result;
        }

        [HttpPost]
        [Route("add")]
        [PermissionFilter(AuthorizeCode.Menu.Add)]
        public async Task<ResultDto<bool>> Create([FromBody] CreateUpdateMenuDto input)
        {
            var result = new ResultDto<bool>();
            //var query = await _menuRepository.GetOneAsync(p => p.Title.Equals(input.Title) && p.IsDelete == false);
            //if (query != null)
            //{
            //    result.Msg = "菜单已存在";
            //    return result;
            //}

            var model = _mapper.Map<Menu>(input);
            model.CreateBy = LoginUser.UserName;

            var entity = await _menuRepository.AddAsync(model);
            result.SetData(entity.Id > 0);
            return result;
        }

        [HttpPost]
        [Route("edit")]
        [PermissionFilter(AuthorizeCode.Menu.Edit)]
        public async Task<ResultDto<bool>> Update([FromBody] CreateUpdateMenuDto input)
        {
            var result = new ResultDto<bool>();

            //var query = await _menuRepository.GetOneAsync(p => p.Name.Equals(input.Name) && p.Id != input.Id && p.IsDelete == false);
            //if (query != null)
            //{
            //    result.Msg = "Name已存在";
            //    return result;
            //}

            var entity = _menuRepository.GetQueryable(p => p.Id == input.Id && p.IsDelete == false).AsNoTracking().FirstOrDefault();
            if (entity == null)
            {
                result.Msg = "数据不存在";
                return result;
            }

            var model = _mapper.Map<Menu>(input);

            model.CreationTime = entity.CreationTime;
            model.UpdateBy = LoginUser.UserName;
            model.UpdateTime = DateTime.Now;

            var res = await _menuRepository.UpdateAsync(model);
            result.SetData(res);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        [PermissionFilter(AuthorizeCode.Menu.Delete)]
        public async Task<ResultDto<bool>> Delete(int[] ids)
        {
            var result = new ResultDto<bool>();

            var list = await _menuRepository.GetListAsync(p => p.IsDelete == false);
            var items = new List<Menu>();
            foreach (var id in ids)
            {
                var query = list.Where(p => p.ParentId == id).ToList();
                var item = list.FirstOrDefault(p => p.Id == id);
                if (query.Count > 0)
                {
                    result.Msg = $"请先删除{item.Title}的子菜单";
                    return result;
                }
                else
                {
                    item.IsDelete = true;
                    items.Add(item);
                }
            }
            var b = await _menuRepository.UpdateAsync(items);
            result.SetData(b);
            return result;
        }

        /// <summary>
        /// 获取菜单按钮权限集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("btn-auths")]
        public ResultDto<List<string>> GetPermissionsCode()
        {
            var result = new ResultDto<List<string>>();
            var permissions = _permissionService.GetPermissionKeys();
            result.SetData(permissions);
            return result;
        }

    }
}
