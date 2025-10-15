using GrowthPulse.Data;
using GrowthPulse.Repositories;
using GrowthPulse.Models;
using System.Collections.Generic;
using System.Linq;

namespace GrowthPulse.Repositories
{
    public class PlantRepository : IPlantRepository
    {
        private readonly ApplicationDbContext _context;

        public PlantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Plant> GetAll() => _context.Plants.ToList();
        public Plant GetById(int id) => _context.Plants.Find(id);
        public void Insert(Plant plant) => _context.Plants.Add(plant);
        public void Update(Plant plantChanges) => _context.Plants.Update(plantChanges);
        public void Delete(int id)
        {
            Plant plant = GetById(id);
            if (plant != null) _context.Plants.Remove(plant);
        }
    }
}