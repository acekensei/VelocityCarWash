using System.ComponentModel.DataAnnotations;

namespace CarWashClient.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }
        public string MethodType { get; set; }
    }
}
