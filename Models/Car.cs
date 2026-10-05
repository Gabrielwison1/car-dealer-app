using System.ComponentModel.DataAnnotations;

namespace CarDealerApp.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        public string Make { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2030)]
        public int Year { get; set; }

        [Range(0, 100000000)]
        public decimal Price { get; set; }

        [Display(Name = "Car Image")]
        public string ImagePath { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}