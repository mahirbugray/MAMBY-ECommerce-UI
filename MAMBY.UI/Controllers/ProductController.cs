using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace MAMBY.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

		public ProductController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
        [HttpGet]
		public async Task<IActionResult> Index(int id)
        {
            var client = _httpClientFactory.CreateClient();
            if(id == 0)
            {
                var result = await client.GetAsync("https://localhost:7266/api/Product/GetAllProducts");
                var jsonData = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
                    return View(data);
                }
            }
            else
            {
                var result = await client.GetAsync("https://localhost:7266/api/Product/GetProductsByCategory/" + id);
                var jsonData = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
                    return View(data);
                }
            }
           
			return RedirectToAction("Index", "Home");
		}
        [HttpGet] 
        public async Task<IActionResult> Details(int id) 
        {
	        var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Product/GetProductById/" + id);
			var jsonData = await result.Content.ReadAsStringAsync();
			if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = JsonConvert.DeserializeObject<ProductViewModel>(jsonData);
                return View(data);
            }
            return RedirectToAction("Index", "Home");           // 404 sayfası yapılınca ona yönlendirme yapılacak.
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsSearch()
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Product/GetProductsSearch");
			var jsonData = await result.Content.ReadAsStringAsync();
			if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
                return View("Index", data);
            }
            return RedirectToAction("Index", "ErrorPage");
        }

	}
}
