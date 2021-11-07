using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
    public class DepParamRepository : IDepParamRepository
    {
        private readonly ApplicationContext _context;

        public DepParamRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<Parameters> AllParameters()
        {
            var parameters = _context.Parameters.Include(u => u.Departments).Include(u => u.evaluations).Include(u => u.ParametersGroups);
            return parameters.ToList();
        }

        public void Delete(int DepartmentId, int ParameterId)
        {
            DepartmentParameters departmentParameters = GetById(DepartmentId, ParameterId);
            _context.Remove(departmentParameters);
            _context.SaveChanges();
        }

        public DepartmentParameters GetById(int DepartmentId, int ParameterId)
        {
            DepartmentParameters departmentParameters = new DepartmentParameters();
            foreach (DepartmentParameters department in _context.DepartmentParameters)
            {
                if ((department.DepartmentId == DepartmentId) & (department.ParameterId == ParameterId))
                {
                    departmentParameters= department;
                }
            }
            return departmentParameters;
        }

        public void Insert(DepartmentParameters departmentParameters)
        {
            _context.Add(departmentParameters);
            _context.SaveChanges();
        }

        public void Update(DepartmentParameters departmentParameters)
        {
            _context.Update(departmentParameters);
            _context.SaveChanges();
        }
    }
}
