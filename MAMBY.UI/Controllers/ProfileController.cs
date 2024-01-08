using MAMBY.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;

namespace MAMBY.UI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.GetString("user");
            if (user != null)
            {
                var userData = JsonConvert.DeserializeObject<UserViewModel>(user);
                return View(userData);
            }
            return RedirectToAction("Login", "Account");   
        }
        [HttpGet]
        public IActionResult UpdateProfile()
		{
			var user = HttpContext.Session.GetString("user");
			var userData = JsonConvert.DeserializeObject<UpdatedUserViewModel>(user);
			return View(userData);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdatedUserViewModel model)
        {
            
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
            var result = await client.PutAsync("https://localhost:7266/api/Profile/UpdateProfile", content);
            var error = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                var updateUser = await result.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<UpdatedUserViewModel>(updateUser);

                UserViewModel updatedUserViewModel = new UserViewModel()
                {
                    Name = userData.Name,
                    Surname = userData.Surname,
                    UserName = userData.UserName,
                    PhoneNumber = userData.PhoneNumber,
                    Address = userData.Address,
                    BirthDate = userData.BirthDate,
                };
                string user = JsonConvert.SerializeObject(updatedUserViewModel);
                HttpContext.Session.SetString("user", user);
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> OrderProfile(int id)
        {
            var userId = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).Id;

            id = Convert.ToInt32(userId);

            string token = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).AccessToken;
            var client = _httpClientFactory.CreateClient();  
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var result = await client.GetAsync("https://localhost:7266/api/Sale/OrderProfile/" + id);
            var jsonData = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = JsonConvert.DeserializeObject<List<SaleViewModel>>(jsonData);

                return View(data);
            }
            return View();
        }
    }
}