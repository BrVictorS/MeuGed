using Microsoft.AspNetCore.Mvc;

namespace SGD.Controllers
{
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
