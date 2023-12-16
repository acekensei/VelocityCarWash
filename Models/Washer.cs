using System.ComponentModel.DataAnnotations;

namespace CarWashClient.Models
{
    public class Washer
    {
        [Key]
        public int Id { get; set; }
        public string WasherName { get; set; }
        public bool IsAvailable { get; set; }
    }
}
