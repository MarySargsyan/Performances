using Perfomans.Models;
using Perfomans.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<User> AllUsers() => _repository.AllUsers();

        public void Delete(int? id)=> _repository.Delete(id);

        public User GetById(int? id) => _repository.GetById(id);

        public User GetUserNmodelsById(int? id) => _repository.GetUserNmodelsById(id);

        public void Insert(User user) => _repository.Insert(user);

        public void Update(User user) => _repository.Update(user);
    }
}
