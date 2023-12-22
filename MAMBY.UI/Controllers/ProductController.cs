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

		public IActionResult Index()
        {
            return View();
        }
        [HttpGet] 
        public async Task<IActionResult> Details(int id) 
        {
	        var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Product/GetProductById/" + id);
            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProductViewModel>(jsonData);
                return View(data);
            }
            return RedirectToAction("Index", "Home");           // 404 sayfası yapılınca ona yönlendirme yapılacak.
        }
    }
}
