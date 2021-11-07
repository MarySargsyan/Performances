using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
    public interface IUserRepository
    {
        List<User> AllUsers();
        List<Departments> AllDepartments();
        List<Role> AllRoles();
        List<State> AllStates();
        User GetById(int? id);
        User GetUserNmodelsById(int? id);
        void Insert(User user);
        void Update(User user);
        void Delete(int? id);
    }
}
