using GrowthPulse.Models;
using System.Collections.Generic;

namespace GrowthPulse.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Insert(User user);
        void Update(User userChanges);
        void Delete(int id);
    }
}