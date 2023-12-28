using MAMBY.UI.Models;
using MAMBY.UI.SessionExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace MAMBY.UI.Controllers
{
    public class CardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        List<CardLineViewModel> card;
        CardLineViewModel order = new CardLineViewModel();
        public IActionResult Index()
        {
            card = GetCards();
            ViewBag.Card = card.Sum(x => x.TotalPrice);
            return View(card);
        }
        public async Task<IActionResult> Add(ProductViewModel model, int quantity)
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                var client = _httpClientFactory.CreateClient();
                var result = await client.GetAsync("https://localhost:7266/api/Product/GetProductById/" + model.Id);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonData = await result.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ProductViewModel>(jsonData);
                    card = GetCards();
                    CardLineViewModel cardLineViewModel = new CardLineViewModel()
                    {
                        ProductId = data.Id,
                        Quantity = quantity,
                        ProductName = data.Name,
                        Price = data.Price,
                        ProductViewModel = data,
                        TotalPrice = 1* data.Price
                    };
                    card = AddCard(card, cardLineViewModel);
                    SaveCard(card);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Index", "ErrorPage");          //404 e döndürülecek.
                }
            }
            else
            {
                //kullanıcı bilgileri eksikse login sayfasına yönlendiriyoruz.
                return RedirectToAction("Login", "Account");
            }
        }
        public IActionResult GetCartAjax()
        {
            var cart = HttpContext.Session.GetJson<List<CardLineViewModel>>("cart") ?? new List<CardLineViewModel>();
            TempData["TotalPrice"] = TotalPrice(cart);
            return PartialView("CartPartial", cart);
        }
        [HttpGet]
        public int Decrease(int id)
        {
            var cart = HttpContext.Session.GetJson<List<CardLineViewModel>>("cart") ?? new List<CardLineViewModel>();
            foreach (var item in cart)
            {
                if (item.ProductId == id)
                {
                    item.Quantity--;
                    if (item.Quantity == 0)
                    {
                        cart.Remove(item);
                        HttpContext.Session.SetJson("cart", cart);
                        return 0;
                    }
                    item.TotalPrice = item.Quantity * item.Price;
                    HttpContext.Session.SetJson("cart", cart);
                    return item.Quantity;
                }
            }
            return 0;
        }
        [HttpGet]
        public int Increase(int id)
        {
            var cart = HttpContext.Session.GetJson<List<CardLineViewModel>>("cart") ?? new List<CardLineViewModel>();
            foreach (var item in cart)
            {
                if (item.ProductId == id)
                {
                    item.Quantity++;
                    item.TotalPrice = item.Quantity * item.Price;
                    HttpContext.Session.SetJson("cart", cart);
                    return item.Quantity;
                }
            }
            return 0;
        }
        public IActionResult Delete(int id)
        {
            card = GetCards();
            card = DeleteCard(card, id);
            SaveCard(card);
            return RedirectToAction("Index");
        }
        public List<CardLineViewModel> GetCards()
        {
            var card = HttpContext.Session.GetJson<List<CardLineViewModel>>("cart") ?? new List<CardLineViewModel>();
            return card;
        }
        public void SaveCard(List<CardLineViewModel> card)
        {
            HttpContext.Session.SetJson("cart", card);
        }
        public IActionResult DeleteCard()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public List<CardLineViewModel> AddCard(List<CardLineViewModel> card, CardLineViewModel cardLine)
        {
            if (card.Any(cd => cd.ProductId == cardLine.ProductId))
            {
                foreach (CardLineViewModel model in card)
                {
                    if (model.ProductId == cardLine.ProductId)
                    {
                        model.Quantity += cardLine.Quantity;
                    }
                }
            }
            else
            {
                card.Add(cardLine);
            }
            return card;
        }
        public List<CardLineViewModel> DeleteCard(List<CardLineViewModel> card, int id)
        {
            card.RemoveAll(cd => cd.ProductId == id);
            return card;
        }
        public int TotalQuantity(List<CardLineViewModel> card)
        {
            var totalQuantity = card.Sum(cd => cd.Quantity);
            return totalQuantity;
        }
        public decimal TotalPrice(List<CardLineViewModel> card)
        {
            var totalPrice = card.Sum(c => c.Quantity * c.Price);
            return totalPrice;
        }
        public IActionResult UpdateCard()
        {
            var cart = HttpContext.Session.GetJson<List<CardLineViewModel>>("cart") ?? new List<CardLineViewModel>();
            return PartialView("CartPartialView", cart);
        }
    }
}


