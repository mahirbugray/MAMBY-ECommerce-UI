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
            TempData["TotalQuantity"] = order.TotalQuantity(card).ToString();
            TempData["TotalPrice"] = order.TotalPrice(card).ToString();
            return View(card);
        }
        public async Task<IActionResult> Add(int id, int quantity)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Product/GetProductById/" + id);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProductViewModel>(jsonData);
                card = GetCards();
                CardLineViewModel cardLineViewModel = new CardLineViewModel();
                order.ProductId = data.Id;
                order.ProductName = data.Name;
                order.Quantity = quantity;
                order.ProductPrice = data.Price;
                card = order.AddCard(card, order);
                SaveCard(card);
                TempData["TotalQuantity"] = order.TotalQuantity(card).ToString();
                TempData["TotalPrice"] = order.TotalPrice(card).ToString();
                return RedirectToAction("Index");
            }
            return View("Index", "ErrorPage");          //404 e döndürülecek.
        }
        public IActionResult Delete(int id)
        {
            card = GetCards();
            card = order.DeleteCard(card, id);
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
    }
}


//Sepete ekleye basıldığında movielerdekinden cardline new lenecek içine ürün bilgileri atılacak. Gidip sessiona bakıp ona göre işlem yapılacak cardline card a eklenip işleme devam edilecek. Alışveriş onayla denildiğinde create card oluşturulacak. saledetail  oluşacak bitince sessiondaki sepet boşaltılacak.


//Sepeteekle metoduna sessiondaki user a bakılacak ona göre ekleme yapılacak user yoksa logine yönlendirme yap.




//[HttpPost]
//public async Task<int> CreateCard(CardViewModel model) 
//{
//    var jsonData = JsonConvert.SerializeObject(model);
//    var client = _httpClientFactory.CreateClient();
//    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
//    var result = await client.PostAsync("https://localhost:7266/api/Card/CreateCart", content);
//    var error = await result.Content.ReadAsStringAsync();
//    if (result.IsSuccessStatusCode)
//    {
//        return model.
//    }
//    ModelState.AddModelError("Error", error);
//}