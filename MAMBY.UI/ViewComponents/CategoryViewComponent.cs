using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var result = await client.GetAsync("https://localhost:7266/api/Category/GetAllCategory");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<CategoryViewModel>>(jsonData);
                return View("Default", data);
            }
            return View("Default", new List<CategoryViewModel>());
        }
    }
}
