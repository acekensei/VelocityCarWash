using System.ComponentModel.DataAnnotations;

namespace CarWashClient.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string CarType { get; set; }
        public double Price { get; set; }
    }
}
