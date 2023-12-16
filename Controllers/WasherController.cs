using CarWashClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CarWashClient.Controllers
{
    public class WasherController : Controller
    {
        const string API_URL = "https://localhost:7268/api";

        Uri baseAddress = new Uri(API_URL);
        private readonly HttpClient _client;

        public WasherController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _client.GetAsync($"{_client.BaseAddress}/washer").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            var washerList = JsonConvert.DeserializeObject<List<Washer>>(data);

            return View(washerList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Washer values)
        {
            if (!ModelState.IsValid) return View(values);

            // Serialize the object and sends it to thru the API to the DB
            var data = JsonConvert.SerializeObject(values);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_client.BaseAddress}/washer", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(values);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            try
            {
                Washer washer = new Washer();

                var response = _client.GetAsync($"{_client.BaseAddress}/washer/{Id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    washer = JsonConvert.DeserializeObject<Washer>(data);
                }
                return View(washer);
            }
            catch (Exception)
            {
                return View();
            };
        }

        [HttpPost]
        public IActionResult Edit(Washer washer)
        {
            if (!ModelState.IsValid) return View(washer);

            try
            {
                string data = JsonConvert.SerializeObject(washer);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/washer/{washer.Id}", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            try
            {
                Washer washer = new Washer();

                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/washer/{Id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    washer = JsonConvert.DeserializeObject<Washer>(data);
                }
                return View(washer);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int Id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/washer/{Id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }

            return View();
        }
    }
}
