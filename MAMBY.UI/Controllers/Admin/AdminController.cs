using Microsoft.AspNetCore.Mvc;

namespace MAMBY.UI.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
