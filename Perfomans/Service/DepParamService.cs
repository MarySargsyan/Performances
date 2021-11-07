using Perfomans.Models;
using Perfomans.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
    public class DepParamService : IDepParamService
    {
        private readonly IDepParamRepository _repository;

        public DepParamService(IDepParamRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Parameters> AllParameters() => _repository.AllParameters();
        public void Delete(int DepartmentId, int ParameterId) => _repository.Delete(DepartmentId, ParameterId);

        public DepartmentParameters GetById(int DepartmentId, int ParameterId) => _repository.GetById(DepartmentId, ParameterId);

        public void Insert(DepartmentParameters departmentParameters) => _repository.Insert(departmentParameters);

        public void Update(DepartmentParameters departmentParameters) => _repository.Update(departmentParameters);
    }
}
