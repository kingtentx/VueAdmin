using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using VueAdmin.Api.Dtos;
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
        private IRepository<Menu> _menuRepository;
        public MenuController(
            IMapper mapper,
            IRepository<Menu> menuRepository
            )
        {
            _mapper = mapper;
            _menuRepository = menuRepository;
        }

        [HttpGet]
        [Route("init-menu")]
        [AllowAnonymous]
        public ResultDto<bool> InitMenu()
        {
            var result = new ResultDto<bool>();
            // 读取 JSON 文件的路径
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/menu_list.json");

            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                result.Msg = "JSON 文件未找到。";
                return result;
            }
            string jsonContent = System.IO.File.ReadAllText(filePath);

            var list = JsonConvert.DeserializeObject<List<Menu>>(jsonContent);
            //_menuRepository.Add(list);
            //var pList = jsonData.Where(p => p.ParentId == 0).ToList();
            //var list = new List<Menu>();
            //foreach (var p in pList)
            //{              
            //    var entity = _menuRepository.Add(p);
            //    if (entity.Mid > 0)
            //    {
            //        var sonList = jsonData.Where(a => a.ParentId == p.Id).ToList();
            //        if (sonList.Any())
            //        {
            //            foreach (var son in sonList)
            //            {                           
            //                //son.ParentId = entity.Id;                           
            //                list.Add(son);
            //            }
            //            _menuRepository.Add(list);
            //        }
            //    }
            //}
            result.Ok();
            return result;
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        [Route("get-menus")]
        public ResultDto<object> GetMenus()
        {
            var result = new ResultDto<object>();

            // 读取 JSON 文件的路径
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/menu.json");
            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                result.Msg = "JSON 文件未找到。";
                return result;
            }
            string jsonContent = System.IO.File.ReadAllText(filePath);
            var jsonData = JsonConvert.DeserializeObject<object>(jsonContent);
            result.SetData(jsonData);

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
            var list = await _menuRepository.GetListAsync(p => p.IsDelete == false);
            var data = _mapper.Map<List<MenuDto>>(list);
            result.SetData(data);
            return result;
        }

        [HttpPost]
        [Route("add")]
        public async Task<ResultDto<bool>> Create([FromBody] CreateUpdateMenuDto input)
        {
            var result = new ResultDto<bool>();
            var query = await _menuRepository.GetOneAsync(p => p.Title.Equals(input.Title) && p.IsDelete == false);
            if (query != null)
            {
                result.Msg = "菜单已存在";
                return result;
            }

            var model = _mapper.Map<Menu>(input);         
            model.CreateBy = LoginUser.UserName;

            var entity = await _menuRepository.AddAsync(model);
            result.SetData(entity.Id > 0);
            return result;
        }

        [HttpPost]
        [Route("edit")]
        public async Task<ResultDto<bool>> Update([FromBody] CreateUpdateMenuDto input)
        {
            var result = new ResultDto<bool>();
            var query = await _menuRepository.GetOneAsync(p => p.Name.Equals(input.Name) && p.Id != input.Id && p.IsDelete == false);
            if (query != null)
            {
                result.Msg = "Name已存在";
                return result;
            }

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

        [HttpPost]
        [Route("delete")]
        public async Task<ResultDto<bool>> Delete(int[] ids)
        {
            var result = new ResultDto<bool>();

            //var list = await _menuRepository.GetListAsync(p => ids.Contains(p.Id));
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

    }
}
