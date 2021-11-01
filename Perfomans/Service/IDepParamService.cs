using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
    public interface IDepParamService
    {
        DepartmentParameters GetById(int DepartmentId, int ParameterId);
        void Insert(DepartmentParameters departmentParameters);
        void Update(DepartmentParameters departmentParameters);
        void Delete(int DepartmentId, int ParameterId);
    }
}
