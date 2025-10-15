using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowthPulse.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } // The total price for the whole order

        // --- REMOVE THESE LINES ---
        // public int ListingId { get; set; }
        // public Listing Listing { get; set; }

        // --- ADD THIS LINE ---
        public ICollection<OrderItem> OrderItems { get; set; } // An order can have many items

        public int BuyerId { get; set; }
        public User Buyer { get; set; }
    }
}