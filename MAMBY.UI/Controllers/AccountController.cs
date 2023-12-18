using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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
        public IActionResult Index()
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
                var jsonDataa = await result.Content.ReadAsStringAsync();
                return RedirectToAction("Index", "Home");   
            }
            return View(model);
        }
    }
}
