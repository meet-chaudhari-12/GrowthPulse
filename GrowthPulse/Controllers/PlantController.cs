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
using System.Security.Claims;
using System.Threading.Tasks;

namespace GrowthPulse.Controllers
{
    [Authorize]
    public class PlantController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PlantController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        // GET: /Plant
        public IActionResult Index()
        {
            var plants = _unitOfWork.Plants.GetAll();
            var plantViewModels = _mapper.Map<List<PlantViewModel>>(plants);
            return View(plantViewModels);
        }

        // --- ADDED: The Details Action ---
        // GET: /Plant/Details/5
        public IActionResult Details(int id)
        {
            var plant = _unitOfWork.Plants.GetById(id);
            if (plant == null)
            {
                return NotFound();
            }

            var plantDetailsViewModel = _mapper.Map<PlantDetailsViewModel>(plant);
            return View(plantDetailsViewModel);
        }

        // GET: /Plant/Create
        [Authorize(Roles ="Admin,Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Plant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin,Manager")]
        public async Task<IActionResult> Create(PlantCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var plant = _mapper.Map<Plant>(viewModel);
                plant.Status = PlantStatus.Seedling;
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                plant.UserId = int.Parse(userId);

                // Handle image upload
                if (viewModel.Photo != null && viewModel.Photo.Length > 0)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(viewModel.Photo.FileName);
                    string extension = Path.GetExtension(viewModel.Photo.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/images/plants/", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await viewModel.Photo.CopyToAsync(fileStream);
                    }
                    plant.PhotoUrl = "/images/plants/" + fileName;
                }

                _unitOfWork.Plants.Insert(plant);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: /Plant/Edit/5
        [Authorize(Roles ="Admin,Manager")]
        public IActionResult Edit(int id)
        {
            var plant = _unitOfWork.Plants.GetById(id);
            if (plant == null)
            {
                return NotFound();
            }

            // Map the Plant entity to your Edit ViewModel
            var viewModel = _mapper.Map<PlantEditViewModel>(plant);

            return View(viewModel);
        }

        // POST: /Plant/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin,Manager")]
        public async Task<IActionResult> Edit(int id, PlantEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // 1. Get the existing plant from the database
                var plantToUpdate = _unitOfWork.Plants.GetById(viewModel.Id);
                if (plantToUpdate == null)
                {
                    return NotFound();
                }

                // 2. Handle the new image upload, if there is one
                if (viewModel.Photo != null && viewModel.Photo.Length > 0)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    // --- DELETION OF OLD IMAGE (IMPORTANT) ---
                    if (!string.IsNullOrEmpty(plantToUpdate.PhotoUrl))
                    {
                        // Construct the full path to the old image
                        var oldImagePath = Path.Combine(wwwRootPath, plantToUpdate.PhotoUrl.TrimStart('/'));
                        // Check if the file exists and delete it
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    // --- END OF DELETION ---

                    // --- SAVE THE NEW IMAGE (Same logic as your Create method) ---
                    string fileName = Path.GetFileNameWithoutExtension(viewModel.Photo.FileName);
                    string extension = Path.GetExtension(viewModel.Photo.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath, "images", "plants", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(path)); // Ensure directory exists
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await viewModel.Photo.CopyToAsync(fileStream);
                    }
                    // Update the PhotoUrl with the new path
                    plantToUpdate.PhotoUrl = "/images/plants/" + fileName;
                }

                // 3. Update the rest of the plant's properties from the ViewModel
                plantToUpdate.Name = viewModel.Name;
                plantToUpdate.Species = viewModel.Species;
                plantToUpdate.PlantingDate = viewModel.PlantingDate;
                plantToUpdate.Status = viewModel.Status;

                // 4. Save the changes to the database
                _unitOfWork.Plants.Update(plantToUpdate);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, return to the edit view with the current data
            return View(viewModel);
        }

        // --- ADD THESE TWO DELETE METHODS ---

        // GET: /Plant/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(int id)
        {
            var plant = _unitOfWork.Plants.GetById(id);
            if (plant == null)
            {
                return NotFound();
            }

            // Pass the plant details to a confirmation view.
            var viewModel = _mapper.Map<PlantViewModel>(plant);
            return View(viewModel);
        }

        // POST: /Plant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult DeleteConfirmed(int id)
        {
            var plant = _unitOfWork.Plants.GetById(id);
            if (plant == null)
            {
                return NotFound();
            }

            // --- IMPORTANT: Delete the associated image file ---
            if (!string.IsNullOrEmpty(plant.PhotoUrl))
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var imagePath = Path.Combine(wwwRootPath, plant.PhotoUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            // --- End of image deletion ---

            _unitOfWork.Plants.Delete(id);
            _unitOfWork.Save();

            TempData["SuccessMessage"] = $"Plant '{plant.Name}' was deleted successfully.";
            return RedirectToAction(nameof(Index));
        }



    }
}