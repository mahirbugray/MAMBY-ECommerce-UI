using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> Index(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Profile/GetByIdUser/" + id);

            if (result.IsSuccessStatusCode)
            {
                var jsonResponse = await result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserViewModel>(jsonResponse);

                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));

                return View(user);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult UpdateProfile()
        {
            
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserViewModel model)
        {
            
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/Profile/UpdateProfile", content);


            if (result.IsSuccessStatusCode)
            {
                var updateUser = await result.Content.ReadAsStringAsync();
                
                UpdatedUserViewModel updatedUserViewModel = new UpdatedUserViewModel()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    BirthDate = model.BirthDate,
                };
                TempData["updateUser"] = updatedUserViewModel;

                return RedirectToAction("UpdateProfile", "Profile");
            }
            ModelState.AddModelError("", "");
            return View(model);
        }
    }
}