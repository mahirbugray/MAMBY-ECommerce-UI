using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MAMBY.UI.Controllers.Admin
{
    public class AdminSaleController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminSaleController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Sale/GetAllSale");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<SaleViewModel>>(jsonData);
                return View(data);
            }
            return View("Index", "ErrorPage");
        }

        public async Task<IActionResult> SaleDetail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/SaleDetail/GetSaleDetail/" + id);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<SaleDetailViewModel>>(jsonData);
                return View(data);
            }
            return View("Index", "ErrorPage");
        }
    }
}
