using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueAdmin.Api.Dtos;

namespace VueAdmin.Api.Controllers
{
    [Route("api/menu")]
    [ApiController]
    [Authorize]
    public class MenuController : ApiBaseController
    {

        public MenuController(

            )
        {
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        [Route("get-menus")]
        public async Task<ResultDto<object>> GetMenus()
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


    }
}
