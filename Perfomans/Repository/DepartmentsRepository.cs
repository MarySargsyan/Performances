using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
    public class DepartmentsRepository : IDepartmentsRepository
    {
        public static List<User> UsersforExport = new List<User>();
        public static List<ParametersGroup> ParametersGroupsForExport = new List<ParametersGroup>();
        public static int GroupIdForExport;
        private readonly ApplicationContext _context;

        public DepartmentsRepository(ApplicationContext context)
        {
            _context = context;
        }
        public List<Departments> AllDepartments()
        {
            var deparments = _context.Departments.Include(u => u.User).Include(u => u.Groups).Include(u => u.DepartmentParameters);
            return deparments.ToList();
        }

        public List<User> CountTop(int DepId, int GroupId, int EmployeeCount)
        {
            Departments departments = GetById(DepId);
            foreach (User user in departments.User)
            {
                user.result = 0.0;
                foreach (ParametersGroup parametersGroup in _context.ParametersGroups.Where(p => p.GroupId == GroupId).ToList())
                {
                    foreach (Evaluations evaluations in GetLastEvaluations())
                    {
                        if ((parametersGroup.GroupId == GroupId) & (parametersGroup.ParameterId == evaluations.ParameterId) & (evaluations.UserId == user.Id))
                        {
                            user.result += (evaluations.Parameter.Coefficient * evaluations.Mark);
                        }
                    }

                }
            }
            var topsort = from user in departments.User orderby user.result descending select user;
            List<User> CountTop = new List<User>();

            foreach (User u in topsort)
            {
                CountTop.Add(u);
                if (CountTop.Count == EmployeeCount) { break; }
            }
            GroupIdForExport = GroupId;
            ParametersGroupsForExport = _context.ParametersGroups.Where(p => p.GroupId == GroupId).ToList();
            UsersforExport = CountTop;

            return CountTop;
        }

        public void Delete(int? id)
        {
            Departments departments = _context.Departments.Find(id);
            _context.Departments.Remove(departments);
            _context.SaveChanges();
        }

        public Departments DepEmployeesProgreses(int? Id)
        {
            Departments departments = GetById(Id);
            foreach (User u in departments.User)
            {
                u.progress = GetUserProgress(u);
            }
            return departments;
        }

        public Departments GetById(int? id)
        {
            return _context.Departments.Include(d => d.DepartmentParameters).Include(d => d.Groups).Include(d => d.User).FirstOrDefault(d => d.Id == id);
        }

        public List<Evaluations> GetLastEvaluations()
        {
            List<Evaluations> lastevaluations = new List<Evaluations>();
            foreach (Evaluations evaluations in _context.Evaluations.ToList())
            {
                lastevaluations.Add(evaluations);
            }
            foreach (Evaluations laste in lastevaluations.ToList())
            {
                foreach (Evaluations e in _context.Evaluations.ToList())
                {
                    if ((e.UserId == laste.UserId) & (e.ParameterId == laste.ParameterId) & (e.Id < laste.Id))
                    {
                        lastevaluations.Remove(e);
                    }
                }
            }
            return (lastevaluations);
        }

        public List<Evaluations> GetOldEvaluations()
        {
            List<Evaluations> oldevaluations = new List<Evaluations>();
            foreach (Evaluations evaluations in _context.Evaluations.ToList())
            {
                oldevaluations.Add(evaluations);
            }
            foreach (Evaluations e in GetLastEvaluations())
            {
                oldevaluations.Remove(e);
            }
            return (oldevaluations);
        }

        public double GetUserLastEvaluationAvg(User user)
        {
            double Avg = 0.0;
            double count = 0.0;
            double sum = 0.0;
            foreach (Evaluations e in GetLastEvaluations())
            {
                if (e.UserId == user.Id)
                {
                    sum += e.Mark;
                    count += 1;
                }
            }
            Avg = sum / count;
            return Avg;
        }

        public int GetUserProgress(User user)
        {
            user.progress = 0;

            if (GetUsersEvaluationAvg(user) < GetUserLastEvaluationAvg(user))
            {
                user.progress += 1;
            }
            else if (GetUsersEvaluationAvg(user) > GetUserLastEvaluationAvg(user))
            {
                user.progress -= 1;
            }
            else
            {
                user.progress -= 0;
            }

            return user.progress;
        }

        public double GetUsersEvaluationAvg(User user)
        {
            double Avg = 0.0;
            double count = 0.0;
            double sum = 0.0;
            foreach (Evaluations e in GetOldEvaluations())
            {
                if (e.UserId == user.Id)
                {
                    sum += e.Mark;
                    count += 1;
                }
            }
            Avg = sum / count;
            return Avg;
        }

        public void Insert(Departments departments)
        {
            _context.Add(departments);
            _context.SaveChanges();
        }

        public void Update(Departments departments)
        {
            _context.Departments.Update(departments);
            _context.SaveChanges();
        }

        public void WorkbookCreate(XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Top_Emplyee");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Name";
            worksheet.Cell(currentRow, 2).Value = "SourName";
            worksheet.Cell(currentRow, 3).Value = "Parameters";
            worksheet.Cell(currentRow, 4).Value = "Result";
            foreach (User user in UsersforExport)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = user.Name;
                worksheet.Cell(currentRow, 2).Value = user.SourName;
                string Parameters = " ";

                foreach (ParametersGroup parametersGroup in ParametersGroupsForExport)
                {
                    foreach (Evaluations evaluations in GetLastEvaluations())
                    {
                        if ((parametersGroup.GroupId == GroupIdForExport) & (parametersGroup.ParameterId == evaluations.ParameterId) & (evaluations.UserId == user.Id))
                        {
                            Parameters = Parameters + evaluations.Parameter.Name + "- " + evaluations.Mark + "\n";

                        }
                    }
                }
                worksheet.Cell(currentRow, 3).Value = Parameters;
                worksheet.Cell(currentRow, 4).Value = user.result;
            }
        }
    }
}
