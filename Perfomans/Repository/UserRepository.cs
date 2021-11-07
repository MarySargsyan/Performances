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

        public List<Departments> AllDepartments()
        {
                var departments = _context.Departments.Include(u => u.DepartmentParameters).Include(u => u.Groups).Include(u => u.User);
                return departments.ToList();
        }

        public List<Role> AllRoles()
        {
                var roles = _context.Roles.Include(u => u.User);
                return roles.ToList();
        }

        public List<State> AllStates()
        {
                var states = _context.States.Include(u => u.User);
                return states.ToList();
        }

        public List<User> AllUsers()
        {

                var users = _context.User.Include(u => u.Department).Include(u => u.Role).Include(u => u.Supervisor).Include(u => u.state);
                return users.ToList();
        }

        public void Delete(int? id)
        {
           User user = _context.User.Find(id);
            user.StateId = 2;
            _context.User.Update(user);
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
