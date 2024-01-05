using MAMBY.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MAMBY.UI.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            string token = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).AccessToken;
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var result = await client.GetAsync("https://localhost:7266/api/User/GetAllUsers");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<UserViewModel>>(jsonData);
                return View(data);
            }
            return View("Index", "ErrorPage");
        }
    }
}
