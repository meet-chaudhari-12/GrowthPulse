using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowthPulse.Models
{
    public class Listing
    {
        public int Id { get; set; }

        // --- ADDED ---
        // --- ADDED ---
        // Since a listing is no longer tied to one specific plant, it needs its own name.
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } // Keep this property

        // --- ADDED ---
        // It also needs its own image URL for the store page.
        public string PhotoUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Keep this property

        // --- ADDED: This is the most important change ---
        // This tracks how many of this item are available to sell.
        [Required]
        public int StockQuantity { get; set; }

        // -------------------------------------------------------------------
        // --- REMOVED PROPERTIES BELOW ---
        // The following properties are tied to the concept of selling one
        // specific plant, so they are no longer needed.
        // -------------------------------------------------------------------

        // [Required]
        // public DateTime ListingDate { get; set; }

        // public bool IsSold { get; set; } = false;

        // public int PlantId { get; set; }
        // public Plant Plant { get; set; }
    }
}