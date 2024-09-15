using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueAdmin.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        } 
    }
}
