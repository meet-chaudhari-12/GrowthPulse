namespace GrowthPulse.Models
{
 
        public class OrderItem
        {
            public int Id { get; set; }

            // The quantity of the item purchased
            public int Quantity { get; set; }

            // Price of a single unit at the time of purchase
            public decimal UnitPrice { get; set; }

            // Foreign key to the Order
            public int OrderId { get; set; }
            public Order Order { get; set; }
         
            // --- CHANGES ARE IN THE SECTION BELOW ---

            // Foreign key to the Listing (instead of Product)
            public int ListingId { get; set; }    // CHANGED from ProductId
            public Listing Listing { get; set; }  // CHANGED from Product
        }

    
}
