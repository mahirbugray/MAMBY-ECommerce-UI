using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace MAMBY.UI.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetFromJsonAsync<List<CategoryViewModel>>("https://localhost:7266/api/Category/GetAllCategory");
            if (result != null)
            {
                return View(result);
            }
            return View(new List<CategoryViewModel>());
        }
    }
}
