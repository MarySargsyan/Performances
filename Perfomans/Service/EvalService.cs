using Perfomans.Models;
using Perfomans.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
    public class EvalService : IEvalService
    {
        private readonly IEvalRepository _repository;

        public EvalService(IEvalRepository repository)
        {
            _repository = repository;
        }

        public List<Evaluations> AllCorrentAssesorEvaluations(string userName) => _repository.AllCorrentAssesorEvaluations(userName);

        public void Delete(int? id) => _repository.Delete(id);

        public Evaluations GetById(int? id) => _repository.GetById(id);

        public int GetCorrentAssesor(string userName) => _repository.GetCorrentAssesor(userName);

        public void Insert(Evaluations evaluations) => _repository.Insert(evaluations);

        public List<User> MyEmployees(string userName) => _repository.MyEmployees(userName);
    }
}
