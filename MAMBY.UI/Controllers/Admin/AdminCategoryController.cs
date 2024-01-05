using MAMBY.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MAMBY.UI.Controllers.Admin
{
    public class AdminCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminCategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        //[Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            string token = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).AccessToken;
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var result = await client.GetAsync("https://localhost:7266/api/Category/GetAllCategory");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<CategoryViewModel>>(jsonData);
                return View(data);
            }
            return View("Index", "ErrorPage");
        }
    }
}
