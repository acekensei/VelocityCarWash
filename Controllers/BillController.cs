using CarWashClient.Models;
using CarWashClient.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace CarWashClient.Controllers
{
    public class BillController : Controller
    {
        const string API_URL = "https://localhost:7268/api";

        Uri baseAddress = new Uri(API_URL);
        private readonly HttpClient _client;

        public BillController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(int currentPage = 1)
        {
            var response = _client.GetAsync($"{_client.BaseAddress}/bill").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            var billList = JsonConvert.DeserializeObject<List<Bill>>(data);

            var totalRecords = billList.Count(); 
            var pageSize = 10; // Set the page size
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var obj = new BillVM
            {
                Data = billList.AsQueryable(),
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages,
            };

            obj.Data = billList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList().AsQueryable();

            return View(obj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Pull API data for washers
            var washerResponse = _client.GetAsync($"{_client.BaseAddress}/washer").Result;
            var washerData = washerResponse.Content.ReadAsStringAsync().Result;
            List<Washer> washerList = JsonConvert.DeserializeObject<List<Washer>>(washerData)!;

            // Pull API data for vehicle
            var vehicleResponse = _client.GetAsync($"{_client.BaseAddress}/vehicle").Result;
            var vehicleData = vehicleResponse.Content.ReadAsStringAsync().Result;
            List<Vehicle> vehicleList = JsonConvert.DeserializeObject<List<Vehicle>>(vehicleData)!;

            // Pull API data for payment methods
            var pMethodResponse = _client.GetAsync($"{_client.BaseAddress}/paymentmethod").Result;
            var pMethodData = pMethodResponse.Content.ReadAsStringAsync().Result;
            List<PaymentMethod> paymentMethodList = JsonConvert.DeserializeObject<List<PaymentMethod>>(pMethodData)!;

            // To prevent issues, if a washer is not avail then 
            // they will not be displayed in the dropdown list
            var washers = washerList.AsQueryable();
            var availWashers = washers.Where(s => s.IsAvailable == true).ToList();
            

            // Bind properties to the dropdown list
            var viewModel = new BillVM
            {
                WasherList = availWashers.Select(washer => new SelectListItem
                {
                    Text = washer.WasherName,
                    Value = washer.Id.ToString()
                }).ToList(),
                VehicleList = vehicleList.Select(vehicle => new SelectListItem
                {
                    Text = vehicle.CarType,
                    Value = vehicle.Id.ToString()
                }).ToList(),
                PMethodList = paymentMethodList.Select(pMethod => new SelectListItem
                {
                    Text = pMethod.MethodType,
                    Value = pMethod.Id.ToString()
                }).ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(BillVM vm)
        {
            var bill = new Bill
            {
                CustomerName = vm.CustomerName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                IsComplete = vm.IsComplete,
                WasherId = (int)vm.SelectedWasherId,
                VehicleTypeId = (int)vm.SelectedVehicleId,
                PaymentMethodId = (int)vm.SelectedPMethodId,
            };

            // Pull data from api for vehicle and set bill charge

            var vehicleResponse = _client.GetAsync($"{_client.BaseAddress}/vehicle/{bill.VehicleTypeId}").Result;
            var vehicleData = vehicleResponse.Content.ReadAsStringAsync().Result;
            var vehicle = JsonConvert.DeserializeObject<Vehicle>(vehicleData);

            if (bill.VehicleTypeId == vehicle.Id)
            {
                bill.Charge = vehicle.Price;
            }

            // Pull data from api for washer and set washer to not available if they are booked

            var washerGetResponse = _client.GetAsync($"{_client.BaseAddress}/washer/{bill.WasherId}").Result;
            var washerGetData = washerGetResponse.Content.ReadAsStringAsync().Result;
            var selectedWasher = JsonConvert.DeserializeObject<Washer>(washerGetData);

            selectedWasher.IsAvailable = false;

            string washerData = JsonConvert.SerializeObject(selectedWasher);
            StringContent washerContent = new StringContent(washerData, Encoding.UTF8, "application/json");
            HttpResponseMessage WasherPutResponse = _client.PutAsync($"{_client.BaseAddress}/washer/{bill.WasherId}", washerContent).Result;



            // Serialize the object and sends it to thru the API to the DB
            var data = JsonConvert.SerializeObject(bill);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_client.BaseAddress}/bill", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            //Get bill record using Id and deserialize it in a variable
            var response = _client.GetAsync($"{_client.BaseAddress}/bill/{Id}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var bill = JsonConvert.DeserializeObject<Bill>(content);

            // Pull API data for washers
            var washerResponse = _client.GetAsync($"{_client.BaseAddress}/washer").Result;
            var washerData = washerResponse.Content.ReadAsStringAsync().Result;
            List<Washer> washerList = JsonConvert.DeserializeObject<List<Washer>>(washerData)!;

            // Pull API data for vehicle
            var vehicleResponse = _client.GetAsync($"{_client.BaseAddress}/vehicle").Result;
            var vehicleData = vehicleResponse.Content.ReadAsStringAsync().Result;
            List<Vehicle> vehicleList = JsonConvert.DeserializeObject<List<Vehicle>>(vehicleData)!;

            // Pull API data for payment methods
            var pMethodResponse = _client.GetAsync($"{_client.BaseAddress}/paymentmethod").Result;
            var pMethodData = pMethodResponse.Content.ReadAsStringAsync().Result;
            List<PaymentMethod> paymentMethodList = JsonConvert.DeserializeObject<List<PaymentMethod>>(pMethodData)!;

            var viewModel = new BillVM
            {
               CustomerName = bill.CustomerName,
               Email = bill.Email,
               PhoneNumber = bill.PhoneNumber,  
               Charge = bill.Charge,
               IsComplete = bill.IsComplete,
               SelectedWasherId = bill.WasherId,
               SelectedPMethodId = bill.PaymentMethodId,
               SelectedVehicleId = bill.VehicleTypeId,

                WasherList = washerList.Select(washer => new SelectListItem
                {
                    Text = washer.WasherName,
                    Value = washer.Id.ToString()
                }).ToList(),
                VehicleList = vehicleList.Select(vehicle => new SelectListItem
                {
                    Text = vehicle.CarType,
                    Value = vehicle.Id.ToString()
                }).ToList(),
                PMethodList = paymentMethodList.Select(pMethod => new SelectListItem
                {
                    Text = pMethod.MethodType,
                    Value = pMethod.Id.ToString()
                }).ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(BillVM vm)
        {
            var bill = new Bill
            {
                Id = vm.Id,
                CustomerName = vm.CustomerName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                Charge = vm.Charge,
                IsComplete = vm.IsComplete,
                WasherId = (int)vm.SelectedWasherId,
                VehicleTypeId = (int)vm.SelectedVehicleId,
                PaymentMethodId = (int)vm.SelectedPMethodId,
            };

            // Should make washer available once job is complete
            if (bill.IsComplete == true)
            {
                var washerGetResponse = _client.GetAsync($"{_client.BaseAddress}/washer/{bill.WasherId}").Result;
                var washerGetData = washerGetResponse.Content.ReadAsStringAsync().Result;
                var washer = JsonConvert.DeserializeObject<Washer>(washerGetData);

                washer.IsAvailable = true;

                string washerData = JsonConvert.SerializeObject(washer);
                StringContent washerContent = new StringContent(washerData, Encoding.UTF8, "application/json");
                HttpResponseMessage WasherPutResponse = _client.PutAsync($"{_client.BaseAddress}/washer/{bill.WasherId}", washerContent).Result;
            }

            var data = JsonConvert.SerializeObject(bill);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PutAsync($"{_client.BaseAddress}/bill/{bill.Id}", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            try
            {
                Bill bill = new Bill();

                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/bill/{Id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    bill = JsonConvert.DeserializeObject<Bill>(data);
                }
                return View(bill);
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
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/bill/{Id}").Result;

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

        [HttpPost]
        public IActionResult MarkComplete(int billId, int washerId)
        {
            // Get the bill by its ID
            var billResponse = _client.GetAsync($"{_client.BaseAddress}/bill/{billId}").Result;
            var billData = billResponse.Content.ReadAsStringAsync().Result;
            var bill = JsonConvert.DeserializeObject<Bill>(billData);

            // Get the washer by its ID
            var washerResponse = _client.GetAsync($"{_client.BaseAddress}/washer/{washerId}").Result;
            var washerData = washerResponse.Content.ReadAsStringAsync().Result;
            var washer = JsonConvert.DeserializeObject<Washer>(washerData);

            // Mark the bill as complete
            bill.IsComplete = true;

            // Mark the washer as available
            washer.IsAvailable = true;

            // Update the bill in the API
            var updatedBillData = JsonConvert.SerializeObject(bill);
            StringContent billContent = new StringContent(updatedBillData, System.Text.Encoding.UTF8, "application/json");
            var billUpdateResponse = _client.PutAsync($"{_client.BaseAddress}/bill/{billId}", billContent).Result;

            // Update the washer in the API
            var updatedWasherData = JsonConvert.SerializeObject(washer);
            StringContent washerContent = new StringContent(updatedWasherData, System.Text.Encoding.UTF8, "application/json");
            var washerUpdateResponse = _client.PutAsync($"{_client.BaseAddress}/washer/{washerId}", washerContent).Result;

            return RedirectToAction("Index"); // Redirect to the bill list after successfully marking as complete and making the washer available
        }

        [HttpGet]
        public IActionResult ViewBill(int Id)
        {
            var response = _client.GetAsync($"{_client.BaseAddress}/bill/{Id}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var bill = JsonConvert.DeserializeObject<Bill>(content);

            return View(bill);
        }
    }
}
