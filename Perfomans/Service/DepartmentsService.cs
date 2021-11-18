using ClosedXML.Excel;
using Perfomans.Models;
using Perfomans.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IDepartmentsRepository _repository;

        public DepartmentsService(IDepartmentsRepository repository)
        {
            _repository = repository;
        }

        public List<Departments> AllDepartments() => _repository.AllDepartments();

        public List<User> CountTop(int DepId, int GroupId, int EmployeeCount) => _repository.CountTop(DepId, GroupId, EmployeeCount);

        public void Delete(int? id) => _repository.Delete(id);

        public Departments DepEmployeesProgreses(int? Id) => _repository.DepEmployeesProgreses(Id);

        public Departments GetById(int? id) => _repository.GetById(id);

        public Evaluations GetLastEvaluations() => _repository.GetLastEvaluations();

        public List<Evaluations> GetOldEvaluations() => _repository.GetOldEvaluations();

        public double GetUserLastEvaluationAvg(User user) => _repository.GetUserLastEvaluationAvg(user);

        public int GetUserProgress(User user) => _repository.GetUserProgress(user);

        public double GetUsersEvaluationAvg(User user) => _repository.GetUsersEvaluationAvg(user);

        public void Insert(Departments departments) => _repository.Insert(departments);

        public void Update(Departments departments) => _repository.Update(departments);

        public void WorkbookCreate(XLWorkbook workbook) => _repository.WorkbookCreate(workbook);
    }
}
