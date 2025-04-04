using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using VueAdmin.Api.Dtos;

namespace VueAdmin.Api.Controllers
{
    [Route("api/role")]
    [ApiController]
    [Authorize]
    public class RoleController : ApiBaseController
    {
        public RoleController()
        {

        }

        [HttpGet]
        [Route("list")]
        public async Task<ResultDto<object>> GetRoleLis()
        {
            var result = new ResultDto<object>();

            // 读取 JSON 文件的路径
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/role.json");

            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                result.Msg = "JSON 文件未找到。";
                return result;
            }
            var jsonContent = System.IO.File.ReadAllText(filePath);

            var jsonData = JsonObject.Parse(jsonContent);
            var data = JsonConvert.DeserializeObject<object>(jsonData["all-role"].ToString());
            result.SetData(data);
            return result;
        }
    }
}
