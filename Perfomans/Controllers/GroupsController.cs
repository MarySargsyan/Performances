using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using Perfomans.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupService _service;
        public GroupsController(IGroupService service)
        {
            _service = service;
        }
        public IActionResult Create(int DepId)
        {
            ViewBag.DepId = DepId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Groups groups, int DepId)
        {
            groups.DepartmentId = DepId;
            if (ModelState.IsValid)
            {
                _service.Insert(groups);
                return RedirectToAction("DepartmentPage", "Departments", new { id = DepId });
            }
            return View(groups);
        }

        public IActionResult Edit(int? id, int DepId)
        {
            ViewBag.DepId = DepId;
            ViewBag.Parameters = _service.AllParameters();
            var Parameters = _service.AllParameters();
            ViewBag.SelectedItems = Parameters.ToList().Where(i => i.ParametersGroups.Where(p => p.GroupId == id).Count() > 0 ? true : false).ToList();
            Groups groups = _service.GetById(id);
            groups.ParametersGroups = _service.AllParametersGroups();
            return View(groups);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Groups groups, int[] selectedItems, int DepId)
        {
            Groups editgroup = _service.Update(groups, selectedItems, DepId);
            return RedirectToAction("EditGroupsParam", new { id =editgroup.id, DepId= DepId});
        }
        public IActionResult EditGroupsParam(int? id, int DepId)
        {
            ViewBag.DepId = DepId;
            ViewBag.Parameters = _service.AllParameters();
            Groups groups = _service.GetById(id);
            groups.ParametersGroups = _service.AllParametersGroups();
            var Parameters = _service.AllParameters();
            ViewBag.SelectedItems = Parameters.ToList().Where(i => i.ParametersGroups.Where(p => p.GroupId == id).Count() > 0 ? true : false).ToList();
            return View(groups);
        }
        public IActionResult Delete(int? id, int DepId)
        {
            ViewBag.DepId = DepId;
            Groups groups = _service.GetById(id);
            return View(groups);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, int DepId)
        {
            _service.Delete(id);
            return RedirectToAction("DepartmentPage","Departments", new { id = DepId}) ;
        }

        public IActionResult Excel(int DepId)
        {
            using (var workbook = new XLWorkbook())
            {
                _service.WorkbookCreate(workbook, DepId);
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File( content,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Groups.xlsx");
                }
            }
        }

    }
}
