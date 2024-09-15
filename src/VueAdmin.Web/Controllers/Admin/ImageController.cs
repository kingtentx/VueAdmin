using Microsoft.AspNetCore.Authorization;
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
    public class ImageController : Controller
    {
        private IRepository<PictureGallery> _pictureService;
        public ImageController(IRepository<PictureGallery> pictureService)
        {
            _pictureService = pictureService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetImageList(string keywords, int pageIndex = 1, int pageSize = 10)
        {
            var result = new ResultModel<List<PictureGallery>>();

            //var name = HttpContext.Request.Query["keywords"].ToString() ?? "";

            var where = LambdaHelper.True<PictureGallery>();

            if (!string.IsNullOrWhiteSpace(keywords))
                where = where.And(p => p.ImageName.Contains(keywords));

            var query = await _pictureService.GetListAsync(where, p => p.Id, pageIndex, pageSize);

            result.Code = (int)ResultCode.Success;
            result.Message = "成功";
            result.Count = query.Count;
            result.Data = query.List;

            return Json(result);
        }
    }
}
