using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MAMBY.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var jsonData = JsonConvert.SerializeObject(model);
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/Account/Login", content);
            if (result.IsSuccessStatusCode)
            {
				return RedirectToAction("Index", "Home");   
            }
			var jsonDataa = await result.Content.ReadAsStringAsync();
			ModelState.AddModelError("Error", jsonDataa);
			return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var jsonData = JsonConvert.SerializeObject(model);
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/Account/Register", content);
            var error = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("Error", error);
            return View(model);
        }
    }
}
