using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashClient.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int WasherId { get; set; }
        public int VehicleTypeId { get; set; }
        public double Charge { get; set; }
        public int PaymentMethodId { get; set; }
        public bool IsComplete { get; set; }

        // Navigtion
        public virtual Washer Washer { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
