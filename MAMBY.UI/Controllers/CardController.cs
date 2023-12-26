using MAMBY.UI.Models;
using MAMBY.UI.SessionExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            TempData["TotalQuantity"] = TotalQuantity(card).ToString();
            TempData["TotalPrice"] = TotalPrice(card).ToString();
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
                        ProductViewModel = data
                    };
                    card = AddCard(card, cardLineViewModel);
                    SaveCard(card);

                    TempData["TotalQuantity"] = TotalQuantity(card).ToString();
                    TempData["TotalPrice"] = TotalPrice(card).ToString();

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

        public IActionResult Delete(int id)
        {
            card = GetCards();
            card = DeleteCard(card, id);
            SaveCard(card);
            return RedirectToAction("Index");
        }
        public List<CardLineViewModel> GetCards()
        {
            var card = HttpContext.Session.GetJson<List<CardLineViewModel>>("card") ?? new List<CardLineViewModel>();
            return card;
        }
        public void SaveCard(List<CardLineViewModel> card)
        {
            HttpContext.Session.SetJson("card", card);
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
    }
}


//Sepete ekleye basıldığında movielerdekinden cardline new lenecek içine ürün bilgileri atılacak. Gidip sessiona bakıp ona göre işlem yapılacak cardline card a eklenip işleme devam edilecek. Alışveriş onayla denildiğinde create card oluşturulacak. saledetail  oluşacak bitince sessiondaki sepet boşaltılacak.


//Sepeteekle metoduna sessiondaki user a bakılacak ona göre ekleme yapılacak user yoksa logine yönlendirme yap.

