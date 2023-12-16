using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarWashClient.Models.ViewModels
{
    public class BillVM
    {
        public IQueryable<Bill> Data { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }


        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double Charge { get; set; }
        public bool IsComplete { get; set; }

        // Dropdownlists
        public List<SelectListItem> WasherList { get; set; }
        public List<SelectListItem> VehicleList { get; set; }
        public List<SelectListItem> PMethodList { get; set; }

        //Selected Properties
        public int SelectedWasherId { get; set; }
        public int SelectedVehicleId { get; set; }
        public int SelectedPMethodId { get; set; }
    }
}
