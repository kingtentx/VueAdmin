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

            //var model = _mapper.Map<Department>(input);
            //model.CreateBy = LoginUser.UserName;

            //var entity = await _deptRepository.AddAsync(model);
            //result.SetData(entity.Id > 0);
            //return result;

            try
            {
                // 生成CascadeId
                var cascadeId = await GenerateCascadeIdAsync(input.ParentId);

                var model = _mapper.Map<Department>(input);
                model.CascadeId = cascadeId;
                model.Leaf = true; // 新增分类默认无子节点
                await _deptRepository.AddAsync(model);

                // 更新父级的Leaf状态
                if (input.ParentId != 0)
                {
                    var parent = await _deptRepository.GetOneAsync(c => c.Id == input.ParentId);
                    if (parent != null && parent.Leaf)
                    {
                        parent.Leaf = false;
                        await _deptRepository.UpdateAsync(parent);
                    }
                }

                result.SetData(true);
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
            }
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

            //var entity = _deptRepository.GetQueryable(p => p.Id == input.Id && p.IsDelete == false).AsNoTracking().FirstOrDefault();
            //if (entity == null)
            //{
            //    result.Msg = "数据不存在";
            //    return result;
            //}

            //var model = _mapper.Map<Department>(input);

            //model.CreationTime = entity.CreationTime;
            //model.UpdateBy = LoginUser.UserName;
            //model.UpdateTime = DateTime.Now;

            //var res = await _deptRepository.UpdateAsync(model);
            //result.SetData(res);
            //return result;


            if (input == null || input.Id != input.Id)
            {
                result.Msg = "请求数据无效";
                return result;
            }

            try
            {
                var existingCategory = await _deptRepository.GetOneAsync(c => c.Id == input.Id && c.IsDelete == false);
                if (existingCategory == null)
                {
                    result.Msg = "数据不存在";
                    return result;
                }

                var originalPid = existingCategory.ParentId;
                var pidChanged = existingCategory.ParentId != input.ParentId;

                // 更新父级关系
                if (pidChanged)
                {
                    existingCategory.ParentId = input.ParentId;
                    // 生成新CascadeId
                    var newCascadeId = await GenerateCascadeIdAsync(input.ParentId);
                    await UpdateCategoryAndChildrenAsync(existingCategory, newCascadeId);
                }

                // 更新其他属性
                existingCategory.Name = input.Name;               
                existingCategory.Sort = input.Sort;
                existingCategory.IsActive = input.Status;

                // 自动更新Leaf状态
                var hasChildren = await _deptRepository.GetOneAsync(c => c.ParentId == existingCategory.Id);
                existingCategory.Leaf = hasChildren != null;

                await _deptRepository.UpdateAsync(existingCategory);

                // 处理父级Leaf状态
                if (pidChanged)
                {
                    // 更新原父级
                    if (originalPid != 0)
                    {
                        var originalParent = await _deptRepository.GetOneAsync(c => c.Id == originalPid);
                        if (originalParent != null)
                        {
                            var hasOriginalChildren = await _deptRepository.GetOneAsync(c => c.ParentId == originalPid);
                            originalParent.Leaf = hasOriginalChildren != null;
                            await _deptRepository.UpdateAsync(originalParent);
                        }
                    }

                    // 更新新父级
                    if (input.ParentId != 0)
                    {
                        var newParent = await _deptRepository.GetOneAsync(c => c.Id == input.ParentId);
                        if (newParent != null && newParent.Leaf)
                        {
                            newParent.Leaf = false;
                            await _deptRepository.UpdateAsync(newParent);
                        }
                    }
                }

                result.SetData(true);
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
            }
            return result;
        }

        private async Task<string> GenerateCascadeIdAsync(int pid)
        {
            if (pid == 0)
            {
                var categories = await _deptRepository.GetListAsync(p => p.ParentId == 0);
                var maxId = categories.Max(c => c.CascadeId);
                return string.IsNullOrEmpty(maxId) ? "001" : (int.Parse(maxId) + 1).ToString("D3");
            }

            var parent = await _deptRepository.GetOneAsync(c => c.Id == pid);
            if (parent == null) throw new Exception("Parent category not found");

            var siblings = await _deptRepository.GetListAsync(c => c.ParentId == pid);
            var lastSibling = siblings.OrderByDescending(c => c.CascadeId).FirstOrDefault();

            var newSegment = lastSibling == null
                ? "001"
                : (int.Parse(lastSibling.CascadeId.Split('.').Last()) + 1).ToString("D3");

            return $"{parent.CascadeId}.{newSegment}";
        }

        private async Task UpdateCategoryAndChildrenAsync(Department category, string newCascadeId)
        {
            var originalCascadeId = category.CascadeId;
            category.CascadeId = newCascadeId;
            await _deptRepository.UpdateAsync(category);

            var children = await _deptRepository.GetListAsync(c => c.ParentId == category.Id);
            foreach (var child in children)
            {
                var childNewCascadeId = child.CascadeId.Replace(originalCascadeId, newCascadeId);
                await UpdateCategoryAndChildrenAsync(child, childNewCascadeId);
            }
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
