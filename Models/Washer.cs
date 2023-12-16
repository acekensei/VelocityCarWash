using System.ComponentModel.DataAnnotations;

namespace CarWashAPI.Models
{
    public class Washer
    {
        [Key]
        public int Id { get; set; }
        public string WasherName { get; set; }
        public bool IsAvailable { get; set; }
    }
}
