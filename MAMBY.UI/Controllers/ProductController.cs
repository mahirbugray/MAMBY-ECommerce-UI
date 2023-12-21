using Microsoft.AspNetCore.Mvc;

namespace MAMBY.UI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
