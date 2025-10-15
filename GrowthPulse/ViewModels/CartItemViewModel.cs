using System.ComponentModel.DataAnnotations;

namespace GrowthPulse.ViewModels
{
    public class CartItemViewModel
    {
        public int ListingId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PhotoUrl { get; set; }
        public int StockQuantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
