using GrowthPulse.Data;
using GrowthPulse.Models;
using GrowthPulse.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GrowthPulse.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAll() => _context.Orders.ToList();
        public Order GetById(int id) => _context.Orders.Find(id);
        public void Insert(Order order) => _context.Orders.Add(order);
        public void Update(Order orderChanges) => _context.Orders.Update(orderChanges);
        public void Delete(int id)
        {
            Order order = GetById(id);
            if (order != null) _context.Orders.Remove(order);
        }

        public Order GetOrderWithDetails(int orderId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Listing)
                .FirstOrDefault(o => o.Id == orderId);
        }
    }
}