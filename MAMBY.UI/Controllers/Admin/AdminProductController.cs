using MAMBY.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MAMBY.UI.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class AdminProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            string token = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).AccessToken;
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var result = await client.GetAsync("https://localhost:7266/api/Product/GetAllFilter");
            var error = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
                return View(data);
            }
            return RedirectToAction("Index", "ErrorPage");
        }
        public async Task<IActionResult> AddProduct()
        {
            string token = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("user")).AccessToken;
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var result = await client.GetAsync("https://localhost:7266/api/Category/GetAllCategory");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<CategoryViewModel>>(jsonData);
                ViewBag.categories = new SelectList(data, "Id", "CategoryName");
                return View();
            }
            return RedirectToAction("Index", "ErrorPage");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model, IFormFile formFile, IFormFile formFile2, IFormFile formFile3, IFormFile formFile4, IFormFile formFile5)
        {
            if (formFile != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", formFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                formFile.CopyTo(stream);
                model.ThumbnailImage = "/images/" + formFile.FileName;
            }
            if (formFile2 != null)
            {
                var path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", formFile2.FileName);
                var stream2 = new FileStream(path2, FileMode.Create);
                formFile2.CopyTo(stream2);
                model.ContentImage = "/images/" + formFile2.FileName;
            }
            if (formFile3 != null)
            {
                var path3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", formFile3.FileName);
                var stream3 = new FileStream(path3, FileMode.Create);
                formFile3.CopyTo(stream3);
                model.ContentImage2 = "/images/" + formFile3.FileName;
            }
            if (formFile4 != null)
            {
                var path4 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", formFile4.FileName);
                var stream4 = new FileStream(path4, FileMode.Create);
                formFile4.CopyTo(stream4);
                model.ContentImage3 = "/images/" + formFile4.FileName;
            }
            if (formFile5 != null)
            {
                var path5 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", formFile5.FileName);
                var stream5 = new FileStream(path5, FileMode.Create);
                formFile5.CopyTo(stream5);
                model.ContentImage4 = "/images/" + formFile5.FileName;
            }
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Brand = model.Brand,
                CategoryId = model.CategoryId,
                Commands = model.Commands,
                ContentImage = model.ContentImage,
                ThumbnailImage = model.ThumbnailImage,
                ContentImage2 = model.ContentImage2,
                ContentImage3 = model.ContentImage3,
                ContentImage4 = model.ContentImage4,
                DateTime = DateTime.Now,
                Name = model.Name,
                Stock = model.Stock,
                Description = model.Description,
                IsDeleted = model.IsDeleted,
                Price = model.Price,
                Point = model.Point
            };
            var jsonData = JsonConvert.SerializeObject(productViewModel);
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/Product/AddProduct", content);
            var error = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var id = JsonConvert.DeserializeObject<int>(await result.Content.ReadAsStringAsync());

                return RedirectToAction("AddProductFeature", "AdminProductFeature", new { id = id, categoryId = model.CategoryId });
            }
            ModelState.AddModelError("Error", error);
            return RedirectToAction("Index", "ErrorPage");
        }
    }
}
