using CarWashClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CarWashClient.Controllers
{
    public class PaymentMethodController : Controller
    {
        const string API_URL = "https://localhost:7268/api";

        Uri baseAddress = new Uri(API_URL);
        private readonly HttpClient _client;

        public PaymentMethodController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _client.GetAsync($"{_client.BaseAddress}/paymentmethod").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            var pMethodList = JsonConvert.DeserializeObject<List<PaymentMethod>>(data);

            return View(pMethodList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PaymentMethod values)
        {
            if (!ModelState.IsValid) return View(values);

            // Serialize the object and sends it to thru the API to the DB
            var data = JsonConvert.SerializeObject(values);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_client.BaseAddress}/paymentmethod", content).Result;

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
                PaymentMethod pMethod = new PaymentMethod();

                var response = _client.GetAsync($"{_client.BaseAddress}/paymentmethod/{Id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    pMethod = JsonConvert.DeserializeObject<PaymentMethod>(data);
                }
                return View(pMethod);
            }
            catch (Exception)
            {
                return View();
            };
        }

        [HttpPost]
        public IActionResult Edit(PaymentMethod pMethod)
        {
            if (!ModelState.IsValid) return View(pMethod);

            try
            {
                string data = JsonConvert.SerializeObject(pMethod);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/paymentmethod/{pMethod.Id}", content).Result;

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
                PaymentMethod pMethod = new PaymentMethod();

                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/paymentmethod/{Id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    pMethod = JsonConvert.DeserializeObject<PaymentMethod>(data);
                }
                return View(pMethod);
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
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/paymentmethod/{Id}").Result;

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
