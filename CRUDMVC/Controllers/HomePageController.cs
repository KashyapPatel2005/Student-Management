using Microsoft.AspNetCore.Mvc;

namespace CRUDMVC.Controllers
{
    public class HomePageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
