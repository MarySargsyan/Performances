using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
    public class UserRepository : IUserRepository
    {
        public ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<User> AllUsers()
        {
            using (_context)
            {
                var users = _context.User.Include(u => u.Department).Include(u => u.Role).Include(u => u.Supervisor).Include(u => u.state);
                return users.ToList();
            }
        }

        public void Delete(int? id)
        {
           User user = _context.User.Find(id);
            _context.User.Remove(user);
            _context.SaveChanges();
        }

        public User GetById(int? id)
        {
            return _context.User.Find(id);
        }

        public User GetUserNmodelsById(int? id)
        {
           return _context.User.Include(u => u.Department).Include(u => u.Role).Include(u => u.Supervisor).Include(u => u.state).FirstOrDefault(m => m.Id == id);
        }

        public void Insert(User user)
        {
            _context.Add(user);
            _context.SaveChanges();

        }

        public void Update(User user)
        {
                _context.User.Update(user);
                _context.SaveChanges();
        }
    }
}
