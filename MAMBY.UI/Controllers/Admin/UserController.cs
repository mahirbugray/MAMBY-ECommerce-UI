using MAMBY.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MAMBY.UI.Controllers.Admin
{
	public class UserController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public UserController(IHttpClientFactory httpClientFactory)
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
				var result = await client.GetAsync("https://localhost:7266/api/User/GetAllUsers");
				if (result.StatusCode == System.Net.HttpStatusCode.OK) 
				{
					var jsonData = await result.Content.ReadAsStringAsync();
					var data = JsonConvert.DeserializeObject<List<UserViewModel>>(jsonData);
					return View(data);
				}
				return View("Index", "ErrorPage");
			}
			return RedirectToAction("Login", "Account");
		}
	}
}
