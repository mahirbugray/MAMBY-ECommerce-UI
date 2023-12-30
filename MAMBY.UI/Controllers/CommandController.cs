using Microsoft.AspNetCore.Mvc;

namespace MAMBY.UI.Controllers
{
    public class CommandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
