using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Perfomans.Models;
using System.Web;
using ClosedXML.Excel;
using System.IO;

namespace Perfomans.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationContext _context;

        public DepartmentsController(ApplicationContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View(_context.Departments.ToList());
        }
         public ActionResult GroupsIndex()
        {
            return View(_context.Groups.ToList());
        }

        public ActionResult DepartmentPage(int? id)
        {
            ViewBag.Evaluations = GetLastEvaluations();
            ViewBag.Parameters = _context.Parameters.ToList();
            ViewBag.ParametersGroups = _context.ParametersGroups.ToList();
            ViewBag.Heads = _context.User.ToList();
            Departments departments = _context.Departments.Find(id);
            departments.DepartmentParameters = _context.DepartmentParameters.ToList();
            departments.Head = _context.Heads.ToList();
            departments.Groups = _context.Groups.ToList();
            departments.User = _context.User.ToList();
            foreach (User user in departments.User)
            {
                user.progress = GetUserProgress(user);
            }
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departments);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departments = await _context.Departments.FindAsync(id);
            if (departments == null)
            {
                return NotFound();
            }
            return View(departments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Departments departments)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentsExists(departments.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(departments);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var departments = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(departments);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departments = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(departments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult EmployeePartial()
        {
            return PartialView();
        }
        public ActionResult GroupsEmployee(int GroupId, int DepId)
        {        
            Departments departments = _context.Departments.Find(DepId);
            ViewBag.Group = _context.Groups.Find(GroupId);
            return View(departments);
        }
        public ActionResult ParametersPartial()
        {
            ViewBag.DepartmentParameters = _context.DepartmentParameters.ToList();
            ViewBag.Parameters = _context.Parameters.ToList();
            return PartialView();
        }
        public ActionResult GroupsPartial()
        {
            ViewBag.Parameters = _context.Parameters.ToList();
            ViewBag.Groups = _context.Groups.ToList();
            return PartialView();
        }

        public static List<User> UsersforExport = new List<User>(); 
        public static List<ParametersGroup> ParametersGroupsForExport = new List<ParametersGroup>();
        public static int GroupIdForExport;
        public ActionResult TopEmployees(int EmployeeCount, int GroupId, int DepId)
        {
            
            Departments departments = _context.Departments.Find(DepId);
            departments.DepartmentParameters = _context.DepartmentParameters.ToList();
            departments.User = _context.User.ToList();

            ViewBag.ParametersGroup = _context.ParametersGroups.Where(p => p.GroupId == GroupId).ToList();
            ViewBag.GroupId = GroupId;
            GroupIdForExport = GroupId;
            ViewBag.Parameters = _context.Parameters.ToList();

            ViewBag.Evaluations = GetLastEvaluations();
            foreach(User user in departments.User)
            {
                user.result = 0.0;
                foreach(ParametersGroup parametersGroup in ViewBag.ParametersGroup)
                        {
                    foreach (Evaluations evaluations in ViewBag.Evaluations)
                    {
                        if ((parametersGroup.GroupId == ViewBag.GroupId) & (parametersGroup.ParameterId == evaluations.ParameterId) & (evaluations.UserId == user.Id))
                        {
                            user.result += (evaluations.Parameter.Coefficient * evaluations.Mark);
                        }
                    }

                }
            }
          var topsort = from user in departments.User orderby user.result descending select user;
           List<User> CountTop = new List<User>();

           foreach(User u in topsort)
           {
             CountTop.Add(u);
             if (CountTop.Count == EmployeeCount){ break;}
           }
            ViewBag.topsort = CountTop;
            UsersforExport = CountTop;
            ParametersGroupsForExport = _context.ParametersGroups.Where(p => p.GroupId == GroupId).ToList();
            return View(departments);

        }
        public IActionResult Excel()
        {
            ViewBag.Parameters = _context.Parameters.ToList();

            using (var workbook = new XLWorkbook())
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

                    foreach(ParametersGroup parametersGroup in ParametersGroupsForExport)
                    {
                        foreach(Evaluations evaluations in GetLastEvaluations())
                        {
                            if ((parametersGroup.GroupId == GroupIdForExport) & (parametersGroup.ParameterId == evaluations.ParameterId) & (evaluations.UserId == user.Id))
                            {
                              Parameters = Parameters +  evaluations.Parameter.Name + "- " +  evaluations.Mark + "\n";

                            }
                        }
                    }
                    worksheet.Cell(currentRow, 3).Value = Parameters;
                    worksheet.Cell(currentRow, 4).Value = user.result;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Top_Emplyee.xlsx");
                }
            }
        }
        public ActionResult ProgressUp(int DepId)
        {
            Departments departments = _context.Departments.Find(DepId);
            departments.DepartmentParameters = _context.DepartmentParameters.ToList();
            departments.User = _context.User.ToList();
            foreach(User u in departments.User)
            {
                u.progress = GetUserProgress(u);
            }
            ViewBag.Parameters = _context.Parameters.ToList();
            ViewBag.Evaluations = GetLastEvaluations();

            return View(departments);

        }
        public ActionResult ProgressDown(int DepId)
        {
            Departments departments = _context.Departments.Find(DepId);
            departments.DepartmentParameters = _context.DepartmentParameters.ToList();
            departments.User = _context.User.ToList();
            foreach(User u in departments.User)
            {
                u.progress = GetUserProgress(u);
            }
            ViewBag.Parameters = _context.Parameters.ToList();
            ViewBag.Evaluations = GetLastEvaluations();

            return View(departments);

        }

        private bool DepartmentsExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
        public List<Evaluations> GetLastEvaluations()//это легко в сервмс можно засунуть
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
        public double GetUsersEvaluationAvg(User user)
        {
            double Avg = 0.0;
            double count = 0.0;
            double sum = 0.0;
            foreach (Evaluations e in GetOldEvaluations())
            {
                if(e.UserId == user.Id)
                {
                    sum += e.Mark;
                    count += 1;
                }
            }
            Avg = sum / count;
            return Avg;
        }
        public double GetUserLastEvaluationAvg(User user)
        {
            double Avg = 0.0;
            double count = 0.0;
            double sum = 0.0;
            foreach (Evaluations e in GetLastEvaluations())
            {
                if(e.UserId == user.Id)
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
    }
}
