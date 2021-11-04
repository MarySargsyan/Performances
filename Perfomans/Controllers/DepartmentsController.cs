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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Perfomans.Service;

namespace Perfomans.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentsService _service;
        private readonly IGroupService _groupService;

        public DepartmentsController(ApplicationContext context, IDepartmentsService service, IGroupService groupService)
        {
            _service = service;
            _groupService = groupService;
        }

        [Authorize(Roles = "admin")]

        public ActionResult Index()
        {
            return View(_service.AllDepartments());
        }
        public ActionResult DepartmentPage(int? id)
        {
            ViewBag.Evaluations = _service.GetLastEvaluations();
            ViewBag.Parameters = _groupService.AllParameters();
            ViewBag.ParametersGroups = _groupService.AllParametersGroups();

            return View(_service.DepEmployeesProgreses(id));
        }

        public IActionResult Create()=> View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                _service.Insert(departments);
                return RedirectToAction(nameof(Index));
            }
            return View(departments);
        }

        public IActionResult Edit(int? id)=> View(_service.GetById(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                _service.Update(departments);
                return RedirectToAction(nameof(Index));
            }
            return View(departments);
        }

        public IActionResult Delete(int? id)=>View(_service.GetById(id));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult EmployeePartial()=>PartialView();
        public ActionResult GroupsEmployee(int GroupId, int DepId)
        {        
            ViewBag.Group = _groupService.GetById(GroupId);
            return View(_service.GetById(DepId));
        }
        public ActionResult ParametersPartial()=> PartialView();
        public ActionResult GroupsPartial()=>PartialView();
        public ActionResult TopEmployees(int EmployeeCount, int GroupId, int DepId)
        {    
            ViewBag.ParametersGroup = _groupService.AllParametersGroups().Where(p => p.GroupId == GroupId).ToList();
            ViewBag.GroupId = GroupId;
            ViewBag.Parameters = _groupService.AllParameters();
            ViewBag.Evaluations = _service.GetLastEvaluations();
            Departments departments = _service.GetById(DepId);
            List<User> CountTop = _service.CountTop(DepId, GroupId, EmployeeCount);
            ViewBag.topsort = CountTop;
            return View(departments);
        }
        public IActionResult Excel()
        {
            ViewBag.Parameters = _groupService.AllParameters();
            using (var workbook = new XLWorkbook())
            {
                _service.WorkbookCreate(workbook);
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Top_Emplyee.xlsx");
                }
            }
        }
        public ActionResult Progress(int DepId, int Direction)
        {
            if (Direction == 1) { ViewBag.Direction = 1;}
            else {ViewBag.Direction = 0;}
            Departments departments = _service.DepEmployeesProgreses(DepId);
            ViewBag.Parameters = _groupService.AllParameters();
            ViewBag.Evaluations = _service.GetLastEvaluations();
            return View(departments);
        }
    }
}
