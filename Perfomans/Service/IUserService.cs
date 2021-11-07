using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
    public interface IUserService
    {
        IEnumerable<User> AllUsers();
        IEnumerable<Departments> AllDepartments();
        IEnumerable<Role> AllRoles();
        IEnumerable<State> AllStates();
        User GetById(int? id);
        User GetUserNmodelsById(int? id);
        void Insert(User user);
        void Update(User user);
        void Delete(int? id);
    }
}
