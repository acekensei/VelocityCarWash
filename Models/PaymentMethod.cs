using System.ComponentModel.DataAnnotations;

namespace CarWashAPI.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }
        public string MethodType { get; set; }
    }
}
