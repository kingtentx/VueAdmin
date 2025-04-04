using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueAdmin.Api.Dtos;

namespace VueAdmin.Api.Controllers
{
    [Route("api/dept")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ApiBaseController
    {
        public DepartmentController()
        {

        }

        [HttpGet]
        [Route("tree-list")]
        public async Task<ResultDto<object>> GetDepartmentLis()
        {
            var result = new ResultDto<object>();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/dept.json");
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
    }
}
