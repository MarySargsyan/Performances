using ClosedXML.Excel;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
    public interface IDepartmentsRepository
    {
        List<Departments> AllDepartments();
        List<UserParamEval> AllUPE();
        Departments GetById(int? id);
        void Insert(Departments departments);
        void Update(Departments departments);
        void Delete(int? id);
        void WorkbookCreate(XLWorkbook workbook);
        List<User> CountTop(int DepId, int GroupId, int EmployeeCount);
        Departments DepEmployeesProgreses(int? Id);
        Evaluations GetLastEvaluations();
        List<Evaluations> GetOldEvaluations();
        double GetUsersEvaluationAvg(User user);
        double GetUserLastEvaluationAvg(User user);
        int GetUserProgress(User user);
    }
}
