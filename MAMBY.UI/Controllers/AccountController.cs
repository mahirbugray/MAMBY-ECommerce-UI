using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
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
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Account/Logout");
            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
				HttpContext.Session.Clear();
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("Index", "Home");
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
                HttpContext.Session.SetString("user", await result.Content.ReadAsStringAsync());
                var user = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user"));
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
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var jsonData = JsonConvert.SerializeObject(model.Username);
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/Account/PasswordResetControl", content);
            if (result.IsSuccessStatusCode)
            {
                var token = await result.Content.ReadAsStringAsync();
                ResetPasswordViewModel getToken = new ResetPasswordViewModel()
                {
                    Token = token,
                    Username = model.Username,
                    Email = model.Email

                };
                return RedirectToAction("SetNewPassword", "Account", getToken);
            }
            ModelState.AddModelError("", "Kullanıcı Adı veya Email hatalı");   
            return View(model);
        }
        [HttpGet]
        public IActionResult SetNewPassword(ResetPasswordViewModel getToken)
        {
            return View(getToken);
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(ResetPasswordViewModel model)
        {
            var jsonData = JsonConvert.SerializeObject(model);
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/Account/PasswordReset", content);
            var error = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("Error", error);
            return RedirectToAction("SetNewPassword", "Account", model);
        }
    }
}
