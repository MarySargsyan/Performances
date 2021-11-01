using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
    public interface IDepParamRepository
    {
        DepartmentParameters GetById(int DepartmentId, int ParameterId);
        void Insert(DepartmentParameters departmentParameters);
        void Update(DepartmentParameters departmentParameters) ;
        void Delete(int DepartmentId, int ParameterId);

    }
}
