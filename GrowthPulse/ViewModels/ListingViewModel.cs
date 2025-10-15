namespace GrowthPulse.ViewModels
{
    public class ListingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }
        public int StockQuantity { get; set; }
    }
}