using MAMBY.UI.Models;
using MAMBY.UI.SessionExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace MAMBY.UI.Controllers
{
    public class SaleController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SaleController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index(string error)
        {
            var cart = HttpContext.Session.GetString("cart");
            var data = JsonConvert.DeserializeObject<List<CardLineViewModel>>(cart);
            ViewBag.TotalPrice = data.Sum(x => x.TotalPrice);
            ViewBag.cart = data;
            var user = HttpContext.Session.GetString("user");
            var userData = JsonConvert.DeserializeObject<UserViewModel>(user);
            if (error != null) 
            {
                ModelState.AddModelError("", error);
            }
            return View(userData);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale()
        {
            var cart = HttpContext.Session.GetJson<List<CardLineViewModel>>("cart");
            decimal totalPrice = cart.Sum(x => x.Price);
            int totalQuantity = cart.Sum(x => x.Quantity);

            SaleViewModel saleViewModel = new SaleViewModel()
            {
                DateTime = DateTime.Now,
                IsDeleted = false,
                TotalPrice = totalPrice,
                TotalQuantity = totalQuantity
            };

            try
            {
                var jsonData = JsonConvert.SerializeObject(saleViewModel);
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
                var result = await client.PostAsync("https://localhost:7266/api/CreateSale/", content);

                if (result.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("cart", await result.Content.ReadAsStringAsync());
                    var data = JsonConvert.DeserializeObject<SaleViewModel>(HttpContext.Session.GetString("cart"));
                    return View("SaleDetails", data);
                }

                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult>ConfirmPayment(SaleDetailViewModel model) //ödeme yap
        {
            TempData["user"] = HttpContext.Session.GetString("user");

            try
            {
				var cart = HttpContext.Session.GetJson<CardLineViewModel>("cart");
				var client = _httpClientFactory.CreateClient();
				var jsonData = JsonConvert.SerializeObject(model);
				var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
				var result = await client.PostAsync("https://localhost:7266/api/CreateSaleDetail/", content);
				if (result.IsSuccessStatusCode)
				{
					var data = JsonConvert.DeserializeObject<SaleDetailViewModel>(jsonData);
					return RedirectToAction("SaleDetail", "Sale");

				}
				else
				{
					return RedirectToAction("Index", new {error ="Ödeme işlemi başarısız oldu. Tekrar deneyin."});
				}
			}
            catch (Exception)
            {
				return View();
			}
            
		}

        [HttpGet]
        public async Task<IActionResult> SaleDetail() // Satış detayı
        {
            return View();
        }
    }
}