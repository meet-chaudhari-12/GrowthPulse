using GrowthPulse.Data;
using GrowthPulse.Repositories;

namespace GrowthPulse.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IPlantRepository _plants;
        private IUserRepository _users;
        private IListingRepository _listings;
        private IOrderRepository _orders;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IPlantRepository Plants => _plants ??= new PlantRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IListingRepository Listings => _listings ??= new ListingRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}