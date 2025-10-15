using GrowthPulse.Models;
using System.Collections.Generic;

namespace GrowthPulse.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order GetById(int id);
        void Insert(Order order);
        void Update(Order orderChanges);
        void Delete(int id);

        Order GetOrderWithDetails(int id);
    }
}