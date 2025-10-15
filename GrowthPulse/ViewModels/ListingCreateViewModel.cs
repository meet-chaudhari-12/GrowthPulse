using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowthPulse.ViewModels
{
    public class ListingCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        // To accept the new uploaded photo
        [Display(Name = "Product Image")]
        public IFormFile Photo { get; set; }

        public string ExistingPhotoUrl {  get; set; }

        public int? PlantId { get; set; }
    }
}