using CarWashClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CarWashClient.Controllers
{
    public class VehicleController : Controller
    {
        const string API_URL = "https://localhost:7268/api";

        Uri baseAddress = new Uri(API_URL);
        private readonly HttpClient _client;

        public VehicleController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _client.GetAsync($"{_client.BaseAddress}/vehicle").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            var vehicleList = JsonConvert.DeserializeObject<List<Vehicle>>(data);

            return View(vehicleList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Vehicle values)
        {
            if (!ModelState.IsValid) return View(values);

            var data = JsonConvert.SerializeObject(values);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_client.BaseAddress}/vehicle", content).Result;

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
                Vehicle vehicle = new Vehicle();

                var response = _client.GetAsync($"{_client.BaseAddress}/vehicle/{Id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    vehicle = JsonConvert.DeserializeObject<Vehicle>(data);
                }
                return View(vehicle);
            }
            catch (Exception)
            {
                return View();
            };
        }

        [HttpPost]
        public IActionResult Edit(Vehicle vehicle)
        {
            if (!ModelState.IsValid) return View(vehicle);

            try
            {
                string data = JsonConvert.SerializeObject(vehicle);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/vehicle/{vehicle.Id}", content).Result;

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
                Vehicle vehicle = new Vehicle();

                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/vehicle/{Id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    vehicle = JsonConvert.DeserializeObject<Vehicle>(data);
                }
                return View(vehicle);
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
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/vehicle/{Id}").Result;

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
