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
            return RedirectToAction("Index");
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
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile formFile)
        {
            if (formFile!= null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\theme\\img", formFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                formFile.CopyTo(stream);
                model.ImageUrl = "/img/" + formFile.FileName /*+ model.Id*/;    //Yüklenen resim isimlerinde çakışma olmaması için ismin sonuna uniq id bilgisini ekliyoruz
                return RedirectToAction("Register", "Account");

            }
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
            var jsonData = JsonConvert.SerializeObject(model);
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/Account/PasswordReset", content);
            var error = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("Error", error);   
            return View(model);
        }
        [HttpGet]
        public IActionResult SetNewPassword(string username, string email)
        {
            var model = new ResetPasswordViewModel
            {
                Username = username,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetNewPassword(ResetPasswordViewModel model)
        {
            // Yeni parola belirleme işlemi yapılacak
            // Eğer başarılıysa giriş sayfasına yönlendirilebilir
            return RedirectToAction("Login", "Account");
        }
    }
}
