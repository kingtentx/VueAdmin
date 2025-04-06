using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VueAdmin.Api.Dtos;
using VueAdmin.Data;
using VueAdmin.Data.ExtModel;
using VueAdmin.Repository;

namespace VueAdmin.Api.Controllers
{
    [Route("api/dept")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ApiBaseController
    {
        private IMapper _mapper;
        private IRepository<Department> _deptRepository;
        public DepartmentController(
            IMapper mapper,
            IRepository<Department> deptRepository
            )
        {
            _mapper = mapper;
            _deptRepository = deptRepository;
        }

        //[HttpGet]
        //[Route("init-dept")]
        //[AllowAnonymous]
        //public ResultDto<bool> InitMenu()
        //{
        //    var result = new ResultDto<bool>();
        //    // 读取 JSON 文件的路径
        //    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/dept.json");

        //    // 检查文件是否存在
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        result.Msg = "JSON 文件未找到。";
        //        return result;
        //    }
        //    string jsonContent = System.IO.File.ReadAllText(filePath);

        //    var list = JsonConvert.DeserializeObject<List<Department>>(jsonContent);
        //    _deptRepository.Add(list);

        //    result.Ok();
        //    return result;
        //}

        [HttpGet]
        [Route("list")]
        public async Task<ResultDto<List<DepartmentDto>>> GetDepartmentLis()
        {
            //var result = new ResultDto<object>();

            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/dept.json");
            //if (!System.IO.File.Exists(filePath))
            //{
            //    result.Msg = "JSON 文件未找到。";
            //    return result;
            //}
            //string jsonContent = System.IO.File.ReadAllText(filePath);

            //var jsonData = JsonConvert.DeserializeObject<object>(jsonContent);
            //result.SetData(jsonData);
            //return result;

            var result = new ResultDto<List<DepartmentDto>>();
            var list = await _deptRepository.GetListAsync(p => p.IsDelete == false);
            var data = _mapper.Map<List<DepartmentDto>>(list);
            result.SetData(data);
            return result;
        }

        [HttpPost]
        [Route("add")]
        public async Task<ResultDto<bool>> Create([FromBody] CreateUpdateDepartmentDto input)
        {
            var result = new ResultDto<bool>();

            //var query = await _deptRepository.GetOneAsync(p => p.Name.Equals(input.Name) && p.IsDelete == false);
            //if (query != null)
            //{
            //    result.Msg = "已存在";
            //    return result;
            //}

            var model = _mapper.Map<Department>(input);
            model.CreateBy = LoginUser.UserName;

            var entity = await _deptRepository.AddAsync(model);
            result.SetData(entity.Id > 0);
            return result;
        }

        [HttpPost]
        [Route("edit")]
        public async Task<ResultDto<bool>> Update([FromBody] CreateUpdateDepartmentDto input)
        {
            var result = new ResultDto<bool>();
            //var query = await _deptRepository.GetOneAsync(p => p.Name.Equals(input.Name) && p.Id != input.Id && p.IsDelete == false);
            //if (query != null)
            //{
            //    result.Msg = "已存在";
            //    return result;
            //}

            var entity = _deptRepository.GetQueryable(p => p.Id == input.Id && p.IsDelete == false).AsNoTracking().FirstOrDefault();
            if (entity == null)
            {
                result.Msg = "数据不存在";
                return result;
            }

            var model = _mapper.Map<Department>(input);

            model.CreationTime = entity.CreationTime;
            model.UpdateBy = LoginUser.UserName;
            model.UpdateTime = DateTime.Now;

            var res = await _deptRepository.UpdateAsync(model);
            result.SetData(res);
            return result;
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ResultDto<bool>> Delete(int[] ids)
        {
            var result = new ResultDto<bool>();

            //var list = await _deptRepository.GetListAsync(p => ids.Contains(p.Id));
            var list = await _deptRepository.GetListAsync(p => p.IsDelete == false);
            var items = new List<Department>();
            foreach (var id in ids)
            {
                var query = list.Where(p => p.ParentId == id).ToList();
                var item = list.FirstOrDefault(p => p.Id == id);
                if (query.Count > 0)
                {
                    result.Msg = $"请先删除{item.Name}的下级部门";
                    return result;
                }
                else
                {
                    item.IsDelete = true;
                    items.Add(item);
                }
            }
            var b = await _deptRepository.UpdateAsync(items);
            result.SetData(b);
            return result;
        }
    }
}
