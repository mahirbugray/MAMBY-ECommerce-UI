using MAMBY.UI.Models;
using MAMBY.UI.SessionExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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
            decimal total = data.Sum(x => x.TotalPrice);
            var user = HttpContext.Session.GetString("user");
            var userData = JsonConvert.DeserializeObject<UserViewModel>(user);
            PaymentViewModel paymentViewModel = new PaymentViewModel()
            {
                User = userData,
                cardLines = data,
                totalPrice = total


            };
            if (error != null)
            {
                ModelState.AddModelError("", error);
            }
            return View(paymentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale()
        {
            var cart = HttpContext.Session.GetJson<List<CardLineViewModel>>("cart");
            decimal totalPrice = cart.Sum(x => x.Price);
            int totalQuantity = cart.Sum(x => x.Quantity);

            try
            {
                var jsonData = JsonConvert.SerializeObject(cart);
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
                var result = await client.PostAsync("https://localhost:7266/api/Sale/CreateSale/", content);

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
        public async Task<IActionResult> ConfirmPayment(PaymentViewModel model, string cardNo, string cardName, string cardMounth, string cardYear, string cardCvv) //ödeme yap
        {
            var cart = HttpContext.Session.GetJson<List<CardLineViewModel>>("cart");
            
            TempData["user"] = HttpContext.Session.GetString("user");
            PaymentPostViewModel paymentPostViewModel = new PaymentPostViewModel()
            {
                cardLines = cart,
                cardMounth = cardMounth,
                cardYear = cardYear,
                cardCvv = cardCvv,
                postCode= model.postCode,
                neighbourhood= model.neighbourhood,
                aptNo = model.aptNo,
                cardName = cardName,
                cardNo = cardNo,
                city= model.city,
                totalPrice = model.totalPrice
            };

            try
            {

                string token = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).AccessToken;      
                var client = _httpClientFactory.CreateClient();  //HttpClient döndürür
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
                var jsonData = JsonConvert.SerializeObject(paymentPostViewModel);
                var content = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
                var result = await client.PostAsync("https://localhost:7266/api/Sale/CreateSale", content);
                var errorMessage = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<SaleDetailViewModel>(jsonData);
                    return RedirectToAction("SaleDetail", "Sale");

                }
                else
                {
                    return RedirectToAction("Index", new { error = "Ödeme işlemi başarısız oldu. Tekrar deneyin." });
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