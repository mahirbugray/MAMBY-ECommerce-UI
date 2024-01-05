using MAMBY.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace MAMBY.UI.Controllers.Admin
{
	public class RoleController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public RoleController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public async Task<IActionResult> Index()
		{
			if (HttpContext.Session.GetString("user") != null)
			{
				var user = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user"));
				var client = _httpClientFactory.CreateClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, user.AccessToken);
				var tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken securityToken = tokenHandler.ReadJwtToken(user.AccessToken);
				var roleClaims = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
				if (roleClaims == null)
				{
					return RedirectToAction("Error", "ErrorPage");
				}
				var result = await client.GetAsync("https://localhost:7266/api/Role/GetAllRoles");
				var error = await result.Content.ReadAsStringAsync();
				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var jsonData = await result.Content.ReadAsStringAsync();
					var data = JsonConvert.DeserializeObject<List<RoleViewModel>>(jsonData);
					return View(data);
				}
				return View("Index", "ErrorPage");
			}
			return RedirectToAction("Login", "Account");
		}
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			if (HttpContext.Session.GetString("user") != null)
			{
				var user = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user"));
				var client = _httpClientFactory.CreateClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, user.AccessToken);
				var tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken securityToken = tokenHandler.ReadJwtToken(user.AccessToken);
				var roleClaims = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
				if (roleClaims == null)
				{
					return RedirectToAction("Error", "ErrorPage");
				}
				var result = await client.GetAsync("https://localhost:7266/api/Role/GetEditRole/" + id);
				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var jsonData = await result.Content.ReadAsStringAsync();
					var data = JsonConvert.DeserializeObject<UsersInOrOutViewModel>(jsonData);
					return View(data);
				}
				return View("Index", "ErrorPage");
			}
			return RedirectToAction("Login", "Account");
		}
		[HttpPost]
		public async Task<IActionResult> Edit(EditRoleViewModel model)
		{
			var jsonData = JsonConvert.SerializeObject(model);
			var client = _httpClientFactory.CreateClient();
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var result = await client.PostAsync("https://localhost:7266/api/Role/PostEditRole", content);
			var error = await result.Content.ReadAsStringAsync();
			if (result.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return View("Index", "Admin");
			}
			return RedirectToAction("Index", "ErrorPage");
		}
	}
}
