using GrowthPulse.Models;
using GrowthPulse.Repositories;
using GrowthPulse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims; // Needed to get the current user's ID
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace GrowthPulse.Controllers
{
    [Authorize(Roles = "Customer, Manager ,Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // This action will be called when a user clicks "Buy Now"
        // It would typically be a POST request from a form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int listingId, int quantity)
        {
            // 1. Find the product (Listing) the user wants to buy
            var listingToBuy = _unitOfWork.Listings.GetById(listingId);
            if (listingToBuy == null)
            {
                return NotFound("Product not found.");
            }

            // 2. Check if there is enough stock
            if (listingToBuy.StockQuantity < quantity)
            {
                // You should show a proper error message to the user on the view
                TempData["ErrorMessage"] = "Sorry, there is not enough stock to complete your order.";
                return RedirectToAction("Index", "Listing");
            }

            // 3. Get the current user's ID
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 4. Create the main Order object
            var order = new Order
            {
                BuyerId = userId,
                PurchaseDate = DateTime.Now,
                OrderItems = new List<OrderItem>(),
                TotalPrice = listingToBuy.Price * quantity // Calculate total price
            };

            // 5. Create the OrderItem2
            var orderItem = new OrderItem
            {
                ListingId = listingId,
                Quantity = quantity,
                UnitPrice = listingToBuy.Price
                
                // Store the price at the time of purchase
            };
            order.OrderItems.Add(orderItem);

            // 6. Reduce the stock quantity of the product
            listingToBuy.StockQuantity -= quantity;
            _unitOfWork.Listings.Update(listingToBuy);

            // 7. Create new Plant records for the user's personal collection
            for (int i = 0; i < quantity; i++)
            {
                var newPlant = new Plant
                {
                    Name = listingToBuy.Name,
                    // You can copy other properties like Species if they exist on your Listing model
                    PlantingDate = DateTime.Now,
                    Status = PlantStatus.Sold,
                    UserId = userId,
                    PhotoUrl = listingToBuy.PhotoUrl, // Copy the photo from the listing
                    ListingId = listingToBuy.Id // Link this plant back to the store product
                };
                _unitOfWork.Plants.Insert(newPlant);
            }

            // 8. Add the new Order to the database
            _unitOfWork.Orders.Insert(order);

            // 9. Save all changes to the database in one transaction
            _unitOfWork.Save();

            // 10. Redirect the user to a success page or their "My Plants" page
            return RedirectToAction("Index","Home"); // Redirecting to the user's plant collection
        }

        // GET: /Order/Checkout
        [Authorize]
        public IActionResult Checkout()
        {
            // For now, redirect to cart since we don't have a full checkout process
            // In a real application, this would handle payment processing
            TempData["InfoMessage"] = "Checkout functionality will be implemented soon. For now, please use the 'Buy Now' button for individual items.";
            return RedirectToAction("Index", "Cart");
        }

        // GET: /Order/History
        [Authorize(Roles = "Customer")]
        public IActionResult History()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Find all orders placed by the current customer
            var orders = _unitOfWork.Orders.GetAll()
                .Where(o => o.BuyerId == userId)
                .OrderByDescending(o => o.PurchaseDate);

            var viewModels = _mapper.Map<List<OrderHistoryViewModel>>(orders);

            return View(viewModels);
        }

        // GET: /Order/Details/5
        [Authorize(Roles = "Customer")]
        public IActionResult Details(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Find the specific order, including its items and the related product details

            var order = _unitOfWork.Orders.GetOrderWithDetails(id);

            if (order == null)
            {
                return NotFound();
            }

            // Security check: Make sure the current user is the one who placed the order
            if (order.BuyerId != userId)
            {
                return Forbid(); // Access denied
            }

            var viewModel = _mapper.Map<OrderDetailViewModel>(order);

            return View(viewModel);
        }


    }
}