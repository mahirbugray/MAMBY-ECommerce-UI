using Microsoft.AspNetCore.Mvc;

namespace MAMBY.UI.Controllers
{
    public class ErrorPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
