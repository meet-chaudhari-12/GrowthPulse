using GrowthPulse.Models;
using GrowthPulse.Repositories;
using GrowthPulse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;

namespace GrowthPulse.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            var cartItems = new List<CartItemViewModel>();

            foreach (var item in cart)
            {
                var listing = _unitOfWork.Listings.GetById(item.ListingId);
                if (listing != null)
                {
                    cartItems.Add(new CartItemViewModel
                    {
                        ListingId = item.ListingId,
                        Name = listing.Name,
                        Price = listing.Price,
                        Quantity = item.Quantity,
                        PhotoUrl = listing.PhotoUrl,
                        StockQuantity = listing.StockQuantity
                    });
                }
            }

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int listingId, int quantity = 1)
        {
            var listing = _unitOfWork.Listings.GetById(listingId);
            if (listing == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            if (listing.StockQuantity < quantity)
            {
                return Json(new { success = false, message = "Not enough stock available" });
            }

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(x => x.ListingId == listingId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem { ListingId = listingId, Quantity = quantity });
            }

            SaveCart(cart);

            return Json(new { success = true, message = "Item added to cart", cartCount = cart.Sum(x => x.Quantity) });
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int listingId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ListingId == listingId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    var listing = _unitOfWork.Listings.GetById(listingId);
                    if (listing != null && listing.StockQuantity >= quantity)
                    {
                        item.Quantity = quantity;
                    }
                    else
                    {
                        return Json(new { success = false, message = "Not enough stock available" });
                    }
                }

                SaveCart(cart);
            }

            return Json(new { success = true, cartCount = cart.Sum(x => x.Quantity) });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int listingId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ListingId == listingId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return Json(new { success = true, cartCount = cart.Sum(x => x.Quantity) });
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return Json(new { success = true, cartCount = 0 });
        }

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            return JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            var count = cart.Sum(x => x.Quantity);
            return Json(new { count = count });
        }
    }

    public class CartItem
    {
        public int ListingId { get; set; }
        public int Quantity { get; set; }
    }
}
