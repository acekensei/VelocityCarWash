using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashAPI.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int WasherId { get; set; }
        public int VehicleTypeId { get; set; }
        public double? Charge { get; set; }
        public int PaymentMethodId { get; set; }
        public bool? IsComplete { get; set; }

        // Relationships
        [ForeignKey("WasherId")]
        public virtual Washer? Washer { get; set; }
        [ForeignKey("VehicleTypeId")]
        public virtual Vehicle? Vehicle { get; set; }
        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}
