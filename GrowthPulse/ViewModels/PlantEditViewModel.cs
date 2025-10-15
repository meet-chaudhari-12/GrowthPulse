using GrowthPulse.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GrowthPulse.ViewModels
{
    public class PlantEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Species { get; set; }

        [Required]
        public DateTime PlantingDate { get; set; }

        [Required]
        public PlantStatus Status { get; set; }
        public string PhotoUrl { get; set; } // To display the current photo
        public IFormFile Photo { get; set; } // To accept the new uploaded photo
    }
}