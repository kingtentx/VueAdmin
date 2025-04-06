using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueAdmin.Api.Dtos;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;
using Role = VueAdmin.Data.Role;

namespace VueAdmin.Api.Controllers
{
    [Route("api/role")]
    [ApiController]
    [Authorize]
    public class RoleController : ApiBaseController
    {
        private IMapper _mapper;
        private IRepository<Role> _roleRepository;
        private IRepository<RoleMenu> _roleMenuRepository;
        private IRepository<Menu> _menuRepository;
        public RoleController(
            IMapper mapper,
            IRepository<RoleMenu> roleMenuRepository,
            IRepository<Role> roleRepository,
            IRepository<Menu> menuRepository
            )
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _roleMenuRepository = roleMenuRepository;
            _menuRepository = menuRepository;
        }

        //[HttpGet]
        //[Route("list")]
        //public async Task<ResultDto<object>> GetRoleList()
        //{
        //    var result = new ResultDto<object>();

        //    // 读取 JSON 文件的路径
        //    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/role.json");

        //    // 检查文件是否存在
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        result.Msg = "JSON 文件未找到。";
        //        return result;
        //    }
        //    var jsonContent = System.IO.File.ReadAllText(filePath);

        //    var jsonData = JsonObject.Parse(jsonContent);
        //    var itmes = JsonConvert.DeserializeObject<List<object>>(jsonData["roles"].ToString());
        //    var data = new ItemList<object>
        //    {
        //        Total = itmes.Count,
        //        List = itmes
        //    };
        //    result.SetData(data);
        //    return result;
        //}

        [HttpPost]
        [Route("list")]
        public async Task<ResultDto<ItemList<RoleDto>>> GetRoleList([FromBody] GetInputRoleDto input)
        {
            var result = new ResultDto<ItemList<RoleDto>>();

            var where = LambdaHelper.True<Role>().And(p => p.IsDelete == false);
            if (!string.IsNullOrEmpty(input.Name))
            {
                where = where.And(p => p.Name.Contains(input.Name));
            }
            if (!string.IsNullOrEmpty(input.Code))
            {
                where = where.And(p => p.Code.Equals(input.Code));
            }
            if (input.Status != null)
            {
                where = where.And(p => p.IsActive == input.Status);
            }

            var items = await _roleRepository.GetListAsync(where, p => p.Id, input.CurrentPage, input.PageSize);

            var data = new ItemList<RoleDto>
            {
                Total = items.Count,
                List = _mapper.Map<List<RoleDto>>(items.List),
                CurrentPage = input.CurrentPage,
                PageSize = input.PageSize
            };

            result.SetData(data);
            return result;
        }

        [HttpPost]
        [Route("add")]
        public async Task<ResultDto<bool>> Create([FromBody] CreateUpdateRoleDto input)
        {
            var result = new ResultDto<bool>();
            var query = await _roleRepository.GetOneAsync(p => p.Name.Equals(input.Name) && p.IsDelete == false);
            if (query != null)
            {
                result.Msg = "用户名已存在";
                return result;
            }

            var model = _mapper.Map<Role>(input);
            model.CreateBy = LoginUser.UserName;

            var entity = await _roleRepository.AddAsync(model);
            result.SetData(entity.Id > 0);
            return result;
        }

        [HttpPost]
        [Route("edit")]
        public async Task<ResultDto<bool>> Update([FromBody] CreateUpdateRoleDto input)
        {
            var result = new ResultDto<bool>();
            var query = await _roleRepository.GetOneAsync(p => p.Name.Equals(input.Name) && p.Id != input.Id && p.IsDelete == false);
            if (query != null)
            {
                result.Msg = "角色名已存在";
                return result;
            }

            var entity = _roleRepository.GetQueryable(p => p.Id == input.Id && p.IsDelete == false).AsNoTracking().FirstOrDefault();
            if (entity == null)
            {
                result.Msg = "用户不存在";
                return result;
            }
            else
            {
                if (entity.Name.Trim() != input.Name.Trim())
                {
                    result.Msg = "用户名不能修改";
                    return result;
                }
            }

            var model = _mapper.Map<Role>(input);

            model.CreationTime = entity.CreationTime;
            model.UpdateBy = LoginUser.UserName;
            model.UpdateTime = DateTime.Now;

            var res = await _roleRepository.UpdateAsync(model);
            result.SetData(res);
            return result;
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ResultDto<bool>> Delete(int[] ids)
        {
            var result = new ResultDto<bool>();

            var list = await _roleRepository.GetListAsync(p => ids.Contains(p.Id));
            var items = new List<Role>();
            foreach (var item in list)
            {
                item.IsDelete = true;
                items.Add(item);
            }
            var b = await _roleRepository.UpdateAsync(items);
            result.SetData(b);
            return result;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ResultDto<List<RoleItemDto>>> GetRoles()
        {
            //var result = new ResultDto<object>();

            //// 读取 JSON 文件的路径
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/role.json");

            //// 检查文件是否存在
            //if (!System.IO.File.Exists(filePath))
            //{
            //    result.Msg = "JSON 文件未找到。";
            //    return result;
            //}
            //var jsonContent = System.IO.File.ReadAllText(filePath);

            //var jsonData = JsonObject.Parse(jsonContent);
            //var data = JsonConvert.DeserializeObject<object>(jsonData["all-role"].ToString());
            //result.SetData(data);

            var result = new ResultDto<List<RoleItemDto>>();
            var list = await _roleRepository.GetListAsync(p =>p.IsActive && p.IsDelete == false);
            var dto = _mapper.Map<List<RoleItemDto>>(list);
            result.SetData(dto);
            return result;
        }

        [HttpGet]
        [Route("role-menu")]
        public async Task<ResultDto<List<MenuItemDto>>> GetRoleMenu()
        {
            //var result = new ResultDto<object>();
            //// 读取 JSON 文件的路径
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/role.json");
            //// 检查文件是否存在
            //if (!System.IO.File.Exists(filePath))
            //{
            //    result.Msg = "JSON 文件未找到。";
            //    return result;
            //}
            //var jsonContent = System.IO.File.ReadAllText(filePath);
            //var jsonData = JsonObject.Parse(jsonContent);
            //var data = JsonConvert.DeserializeObject<object>(jsonData["role-menu"].ToString());
            //result.SetData(data);

            var result = new ResultDto<List<MenuItemDto>>();

            var list = await _menuRepository.GetListAsync(p => p.IsDelete == false);
            var dto = _mapper.Map<List<MenuItemDto>>(list);
            result.SetData(dto);

            return result;
        }

        [HttpGet]
        [Route("role-menu-ids/{id}")]
        public async Task<ResultDto<List<int>>> GetRoleMenuIds(int id)
        {
            //var result = new ResultDto<object>();

            //// 读取 JSON 文件的路径
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/role.json");

            //// 检查文件是否存在
            //if (!System.IO.File.Exists(filePath))
            //{
            //    result.Msg = "JSON 文件未找到。";
            //    return result;
            //}
            //var jsonContent = System.IO.File.ReadAllText(filePath);

            //var jsonData = JsonObject.Parse(jsonContent);
            //var data = JsonConvert.DeserializeObject<object>(jsonData["role-menu-ids"].ToString());
            //result.SetData(data);

            var result = new ResultDto<List<int>>();
            var ids = new List<int>();
            var role = await _roleRepository.GetOneAsync(p => p.Id == id && p.IsDelete == false);
            if (role.IsSuperAdmin)
            {
                //超级管理员显示所有菜单
                ids = (await _menuRepository.GetListAsync(p => p.IsDelete == false)).Select(p => p.Id).ToList();
            }
            else
            {
                ids = (await _roleMenuRepository.GetListAsync(p => p.RoleId == id)).Select(p => p.MenuId).ToList();
            }

            result.SetData(ids);
            return result;
        }

        /// <summary>
        /// 保存角色授权的菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("set-role-menu")]
        public async Task<ResultDto<bool>> UpdateRoleMenu([FromBody] RoleMenuInputDto input)
        {
            var result = new ResultDto<bool>();
           
            //旧的所有角色菜单
            var oldArray = _roleMenuRepository.GetList(p => p.RoleId == input.RoleId).Select(p => p.MenuId).ToArray();
            //当前角色菜单
            var nowArray = input.MenuIds.ToArray();

            var delArray = oldArray.Except(nowArray).ToArray();//新菜单中没有的删除
            var addArray = nowArray.Except(oldArray).ToArray();//旧菜单中没有的新增

            //删除
            if (delArray.Length > 0)
            {
                _roleMenuRepository.Delete(p => p.RoleId == input.RoleId && delArray.Contains(p.MenuId));
            }
            //新增
            if (addArray.Length > 0)
            {
                var list = new List<RoleMenu>();
                foreach (var item in addArray)
                {
                    RoleMenu model = new RoleMenu();
                    model.RoleId = input.RoleId;
                    model.MenuId = item;
                    list.Add(model);
                }
                _roleMenuRepository.Add(list);
            }

            result.SetData(true);
            return result;
        }

    }
}
