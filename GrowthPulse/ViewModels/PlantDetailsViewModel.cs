using System;

namespace GrowthPulse.ViewModels
{
    public class PlantDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime PlantingDate { get; set; }
        public string Status { get; set; }
    }
}