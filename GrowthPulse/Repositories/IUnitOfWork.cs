using System;

namespace GrowthPulse.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPlantRepository Plants { get; }
        IUserRepository Users { get; }
        IListingRepository Listings { get; }
        IOrderRepository Orders { get; }
        int Save();
    }
}