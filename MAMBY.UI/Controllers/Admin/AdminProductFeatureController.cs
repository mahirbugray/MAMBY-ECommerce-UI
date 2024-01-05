using MAMBY.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MAMBY.UI.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class AdminProductFeatureController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminProductFeatureController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> AddProductFeature(int id, int categoryId)
        {
            ViewBag.productId = id;
            string token = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).AccessToken;
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var result = await client.GetAsync("https://localhost:7266/api/ProductFeature/GetAllProductFeature/" + categoryId);
            var error = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<ProductFeatureViewModel>>(jsonData);
                return View(data);
            }
            return RedirectToAction("Index", "ErrorPage");
        }
        [HttpPost]
        public async Task<IActionResult> AddProductFeature(Dictionary<string, string> formData, int productId)
        {
            List<ProductFeatureViewModel> data = new List<ProductFeatureViewModel>();
            foreach (var item in formData)
            {
                if (item.Value != null)
                {
                    ProductFeatureViewModel model = new ProductFeatureViewModel();
                    model.Key = item.Key;
                    model.value = item.Value;
                    model.ProductId = productId;
                    data.Add(model);
                }
            }
            var jsonData = JsonConvert.SerializeObject(data);
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
