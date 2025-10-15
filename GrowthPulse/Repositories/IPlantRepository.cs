using GrowthPulse.Models;
using System.Collections.Generic;

namespace GrowthPulse.Repositories
{
    public interface IPlantRepository
    {
        IEnumerable<Plant> GetAll();
        Plant GetById(int id);
        void Insert(Plant plant);
        void Update(Plant plantChanges);
        void Delete(int id);
    }
}