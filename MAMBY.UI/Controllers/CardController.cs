using MAMBY.UI.Models;
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

        public IActionResult Index()
        {
            return View();
        }
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
    }
}


//Sepete ekleye basıldığında movielerdekinden cardline new lenecek içine ürün bilgileri atılacak. Gidip sessiona bakıp ona göre işlem yapılacak cardline card a eklenip işleme devam edilecek. Alışveriş onayla denildiğinde create card oluşturulacak. saledetail  oluşacak bitince sessiondaki sepet boşaltılacak.


//Sepeteekle metoduna sessiondaki user a bakılacak ona göre ekleme yapılacak user yoksa logine yönlendirme yap.