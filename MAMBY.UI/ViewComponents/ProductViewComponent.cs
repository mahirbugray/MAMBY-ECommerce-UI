using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MAMBY.UI.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Product/GetAllProducts");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
                return View("Default", data);
            }
            return View("Default", new List<ProductViewModel>());
        }
    }
}
