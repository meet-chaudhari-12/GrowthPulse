using GrowthPulse.Data;
using GrowthPulse.Models;
using GrowthPulse.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GrowthPulse.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly ApplicationDbContext _context;

        public ListingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IEnumerable<Listing> GetAll() => _context.Listings.ToList();
        public Listing GetById(int id) => _context.Listings.Find(id);
        public void Insert(Listing listing) => _context.Listings.Add(listing);
        public void Update(Listing listingChanges) => _context.Listings.Update(listingChanges);
        public void Delete(int id)
        {
            Listing listing = GetById(id);
            if (listing != null) _context.Listings.Remove(listing);
        }
        // In both IListingRepository and ListingRepository, update this one method.
        // You will need to add: using Microsoft.EntityFrameworkCore; to ListingRepository.cs

        public IEnumerable<Listing> GetAll()
        {
            return _context.Listings.ToList();
        }
    }
}