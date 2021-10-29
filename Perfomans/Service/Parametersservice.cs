using Perfomans.Models;
using Perfomans.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
    public class Parametersservice : IParametersService
    {
        private readonly IParametersRepository _repository;

        public Parametersservice(IParametersRepository repository)
        {
            _repository = repository;
        }

        public List<Parameters> AllParameters() => _repository.AllParameters();


        public void Delete(int? id) => _repository.Delete(id);

        public Parameters GetById(int? id) => _repository.GetById(id);
        public void Insert(Parameters parameters) => _repository.Insert(parameters);

        public void Update(Parameters parameters) => _repository.Update(parameters);
    }
}
