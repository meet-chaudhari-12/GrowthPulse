using AutoMapper;
using GrowthPulse.Models;
using GrowthPulse.ViewModels;


namespace GrowthPulse.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // For the Index (List) View
            CreateMap<Plant, PlantViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // For the Create Form
            CreateMap<PlantCreateViewModel, Plant>();

            // For the Details View
            CreateMap<Plant, PlantDetailsViewModel>();

            // For the Edit Feature
            CreateMap<Plant, PlantEditViewModel>();
            CreateMap<PlantEditViewModel, Plant>();
            CreateMap<Plant, PlantDeleteViewModel>();

            // For creating the Listing from the form
            CreateMap<ListingCreateViewModel, Listing>();

            // For displaying listings in the marketplace
            CreateMap<Listing, ListingViewModel>()
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Fixed: Use src.Name directly
    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)) // Fixed: Map description
    .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl)); // Fixed: Use src.PhotoUrl directly

            CreateMap<Order, OrderHistoryViewModel>()
    .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.PurchaseDate));

            CreateMap<OrderItem, OrderItemViewModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Listing.Name));

            CreateMap<Order, OrderDetailViewModel>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.PurchaseDate))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}