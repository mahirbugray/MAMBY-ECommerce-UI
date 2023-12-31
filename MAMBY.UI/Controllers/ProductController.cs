using MAMBY.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MAMBY.UI.Controllers
{
	public class ProductController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ProductController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		[HttpGet]
		public async Task<IActionResult> Index(int id, string search)
		{
			var client = _httpClientFactory.CreateClient();
			if (id == 0 && search == null)
			{
				var result = await client.GetAsync("https://localhost:7266/api/Product/GetAllProducts");
				var jsonData = await result.Content.ReadAsStringAsync();
				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var data = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
					return View(data);
				}
			}
			else if (id != 0)
			{
				var result = await client.GetAsync("https://localhost:7266/api/Product/GetProductsByCategory/" + id);
				var jsonData = await result.Content.ReadAsStringAsync();
				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var data = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
					return View(data);
				}
			}
			else if (search != null)
			{
				var result = await client.GetAsync("https://localhost:7266/api/Product/GetProductsSearch/" + search);
				var jsonData = await result.Content.ReadAsStringAsync();
				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var data = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
					return View(data);
				}
			}
			return RedirectToAction("Index", "Product");
		}
		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var result = await client.GetAsync("https://localhost:7266/api/Product/GetProductById/" + id);
			var jsonData = await result.Content.ReadAsStringAsync();
			if (result.StatusCode == System.Net.HttpStatusCode.OK)
			{
				var data = JsonConvert.DeserializeObject<ProductViewModel>(jsonData);
				return View(data);
			}
			return RedirectToAction("Index", "ErrorPage");           // 404 sayfası yapılınca ona yönlendirme yapılacak.
		}
		[HttpPost]
		public async Task<IActionResult> AddCommand(string Content, int product)
		{
			var userId = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).Id;
			CommandViewModel commandViewModel = new CommandViewModel()
			{
				Content = Content,
				DateTime = DateTime.Now,
				ProductId = product,
				UserId = userId
			};
			string token = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).AccessToken;
			var jsonData = JsonConvert.SerializeObject(commandViewModel);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/Command/AddCommand", content);
			var error = await result.Content.ReadAsStringAsync();
			if (result.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return View("Details");
			}
            return RedirectToAction("Index", "ErrorPage");  

        }
    }
}
