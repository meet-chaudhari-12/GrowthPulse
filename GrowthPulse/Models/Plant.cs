using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrowthPulse.Models
{
    public enum PlantStatus
    {
        Seedling,
        Growing,
        Mature,
        ForSale,
        Sold,
        Harvested
    }

    public class Plant
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Species { get; set; }

        public string PhotoUrl { get; set; }

        [Required]
        public DateTime PlantingDate { get; set; }

        public DateTime ExpectedMaturityDate { get; set; }

        [Required]
        public PlantStatus Status { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? ListingId { get; set; }
        public Listing Listing { get; set; }
    }
}