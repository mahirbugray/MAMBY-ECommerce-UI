using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace MAMBY.UI.Controllers.Admin
{
    public class AdminProductFeatureController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminProductFeatureController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult AddProductFeature()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProductFeature(ProductFeatureViewModel model)
        {
            var jsonData = JsonConvert.SerializeObject(model);
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/ProductFeature/AddProductFeature", content);
            var error = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "AdminProduct");
            }
            ModelState.AddModelError("Error", error);
            return RedirectToAction("Index", "ErrorPage");
        }
    }
}
