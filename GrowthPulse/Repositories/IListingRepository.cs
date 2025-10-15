using GrowthPulse.Models;
using System.Collections.Generic;

namespace GrowthPulse.Repositories
{
    public interface IListingRepository
    {
        IEnumerable<Listing> GetAll();
        Listing GetById(int id);
        void Insert(Listing listing);
        void Update(Listing listingChanges);
        void Delete(int id);
    }
}