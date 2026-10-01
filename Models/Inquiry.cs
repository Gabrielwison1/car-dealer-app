using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarDealerApp.Models
{
    public class Inquiry
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; }

        [JsonIgnore]
        [ValidateNever]
        
        public Car? Car { get; set; }
    }
}