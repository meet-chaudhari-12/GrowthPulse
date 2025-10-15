using AutoMapper;
using GrowthPulse.Models;
using GrowthPulse.Repositories;
using GrowthPulse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthPulse.Controllers
{
    [Authorize]
    public class ListingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment; // Inject this for file paths

        public ListingController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        // GET: /Listing (The Public Marketplace)

        public IActionResult Index()
        {
            // --- FIXED ---
            // The logic is now based on StockQuantity, not the old IsSold property.
            var listings = _unitOfWork.Listings.GetAll();
            var viewModels = _mapper.Map<IEnumerable<ListingViewModel>>(listings);
            return View(viewModels);
        }

        // --- COMPLETELY REWRITTEN ---
        // GET: /Listing/Create
        // This is now a simple action for Admins to create a new product from scratch.
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // --- COMPLETELY REWRITTEN ---
        // POST: /Listing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(ListingCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var listing = _mapper.Map<Listing>(viewModel);

                // Handle the image upload
                if (viewModel.Photo != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(viewModel.Photo.FileName);
                    string extension = Path.GetExtension(viewModel.Photo.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath, "images", "listings", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(path)); // Ensure directory exists
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await viewModel.Photo.CopyToAsync(fileStream);
                    }
                    listing.PhotoUrl = "/images/listings/" + fileName;
                }
                else
                {

                    // If no new photo is uploaded, use the existing PhotoUrl from the view model.
                    listing.PhotoUrl = viewModel.ExistingPhotoUrl;
                }

                _unitOfWork.Listings.Insert(listing);

                if (viewModel.PlantId.HasValue)
                {
                    var originalPlant = _unitOfWork.Plants.GetById(viewModel.PlantId.Value);
                    if (originalPlant != null)
                    {
                        originalPlant.Status = PlantStatus.ForSale;
                        _unitOfWork.Plants.Update(originalPlant);
                    }
                }

                //_unitOfWork.Listings.Insert(listing);
                _unitOfWork.Save();

                // You might want to add a success message here using TempData
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }


        [Authorize(Roles = "Admin,Manager")]
        public IActionResult CreateFromPlant(int plantId)
        {
            var plant = _unitOfWork.Plants.GetById(plantId);
            if (plant == null)
            {
                return NotFound();
            }

            // Pre-fill the ViewModel with data from the existing plant
            var viewModel = new ListingCreateViewModel
            {
                PlantId = plant.Id, // We store the original plant's ID
                Name = plant.Name,
                // You can pre-fill the description too
                Description = $"A beautiful {plant.Name} ({plant.Species}).",
                ExistingPhotoUrl = plant.PhotoUrl,
            };

            // We reuse your existing Create.cshtml view by name and pass it the pre-filled data
            return View("Create", viewModel);
        }
    }
}