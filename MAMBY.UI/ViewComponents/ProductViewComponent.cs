using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;

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
            var result = await client.GetFromJsonAsync<List<ProductViewModel>>("https://localhost:7266/api/Product/GetAllProducts");
            if (result != null)
            {
                return View(result);
            }
            return View(new List<ProductViewModel>());
        }
    }
}
