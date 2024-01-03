﻿using MAMBY.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Role/GetAllRoles");
            if(result.StatusCode == System.Net.HttpStatusCode.OK) 
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<RoleViewModel>>(jsonData);
                return View(data);
            }
            return View("Index", "ErrorPage");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://localhost:7266/api/Role/GetEditRole/" + id);
            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UsersInOrOutViewModel>(jsonData);
                return View(data);
            }
            return View("Index", "ErrorPage");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            var jsonData = JsonConvert.SerializeObject(model);
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7266/api/Role/PostEditRole", content);
            var error = await result.Content.ReadAsStringAsync();
            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View("Index", "Admin");
            }
            return RedirectToAction("Index", "ErrorPage");
        }
    }
}
