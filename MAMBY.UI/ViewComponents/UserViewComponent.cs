using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;

namespace MAMBY.UI.ViewComponents
{
    public class UserViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                var loginUser = HttpContext.Session.GetString("user");
                return View(JsonConvert.DeserializeObject<UserViewModel>(loginUser));
            }
            UserViewModel vm = new UserViewModel();
            return View(vm);
        }
    }
}
