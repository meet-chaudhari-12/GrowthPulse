using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GrowthPulse.ViewModels
{
    public class PlantCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Species { get; set; }

        [Required]
        [Display(Name = "Planting Date")]
        public DateTime PlantingDate { get; set; }

        // We'll also need the ID of the user creating the plant
        public int UserId { get; set; }

        // Add for image upload
        public IFormFile Photo { get; set; }
    }
}