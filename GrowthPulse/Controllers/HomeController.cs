using AutoMapper;
using GrowthPulse.Models;
using GrowthPulse.Repositories;
using GrowthPulse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace GrowthPulse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork , IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AllPlants()
        {
            var plants = _unitOfWork.Plants.GetAll();
            var viewModels = _mapper.Map<List<PlantDetailsViewModel>>(plants);
            return View(viewModels);
        }

        // --- ADD THIS NEW ACTION ---
        [Authorize]
        public IActionResult PlantDetails(int id)
        {
            var plant = _unitOfWork.Plants.GetById(id);

            if (plant == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<PlantDetailsViewModel>(plant);

            return View(viewModel);
        }
    }
}
