using MAMBY.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;

namespace MAMBY.UI.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
				var user = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user"));
				var client = _httpClientFactory.CreateClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, user.AccessToken);
				var roleClaims = User.Claims.Where(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
				if (roleClaims == null)
				{
					return RedirectToAction("Error", "ErrorPage");
				}
			}
            return RedirectToAction("Login", "Account");
        }
    }

}
