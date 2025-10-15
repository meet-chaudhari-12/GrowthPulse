using GrowthPulse.Data;
using GrowthPulse.Repositories;
using GrowthPulse.Models;
using System.Collections.Generic;
using System.Linq;

namespace GrowthPulse.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll() => _context.Users.ToList();
        public User GetById(int id) => _context.Users.Find(id);
        public void Insert(User user) => _context.Users.Add(user);
        public void Update(User userChanges) => _context.Users.Update(userChanges);
        public void Delete(int id)
        {
            User user = GetById(id);
            if (user != null) _context.Users.Remove(user);
        }
    }
}