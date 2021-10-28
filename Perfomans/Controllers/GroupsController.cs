using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationContext _context;

        public GroupsController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Create(int DepId)
        {
            ViewBag.DepId = DepId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Groups groups, int DepId)
        {
            groups.DepartmentId = DepId;
            if (ModelState.IsValid)
            {
                _context.Groups.Add(groups);
                await _context.SaveChangesAsync();
                return RedirectToAction("DepartmentPage", "Departments", new { id = DepId });
            }
            return View(groups);
        }

        public async Task<IActionResult> Edit(int? id, int DepId)
        {
            ViewBag.DepId = DepId;
            ViewBag.Parameters = _context.Parameters.ToList();
            ViewBag.SelectedItems = _context.Parameters.Where(i => i.ParametersGroups.Where(p => p.GroupId == id).Count() > 0 ? true : false).ToList();
            Groups groups = await _context.Groups.FindAsync(id);
            groups.ParametersGroups = _context.ParametersGroups.ToList();
            return View(groups);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Groups groups, int[] selectedItems, int DepId)
        {
            Groups editgroup = await _context.Groups.FindAsync(groups.id);
            editgroup.Name = groups.Name;
            editgroup.DepartmentId = DepId;
                foreach (ParametersGroup parametersGroup in _context.ParametersGroups.Where(p => p.GroupId == groups.id))
                {
                    _context.ParametersGroups.Remove(parametersGroup);
                }
                foreach (var i in _context.Parameters.Where(co => selectedItems.Contains(co.Id)))
                {
                     ParametersGroup paramGroup = new ParametersGroup()
                     {
                        ParameterId = i.Id,
                        GroupId = groups.id,
                        Mark = 5
                    };
                    _context.ParametersGroups.Add(paramGroup);
                }

                _context.Entry(editgroup).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            return RedirectToAction("EditGroupsParam", new { id =editgroup.id, DepId= DepId});
        }
        public async Task<IActionResult> EditGroupsParam(int? id, int DepId)
        {
            ViewBag.DepId = DepId;
            Groups groups = await _context.Groups.FindAsync(id);
            groups.ParametersGroups = _context.ParametersGroups.ToList();
            ViewBag.SelectedItems = _context.Parameters.Where(i => i.ParametersGroups.Where(p => p.GroupId == id).Count() > 0 ? true : false).ToList();
            return View(groups);
        }
        public async Task<IActionResult> Delete(int? id, int DepId)
        {
            ViewBag.DepId = DepId;
            Groups groups = await _context.Groups.FindAsync(id);
            return View(groups);
        }

        // POST: Workplaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int DepId)
        {
            Groups groups = await _context.Groups.FindAsync(id);
            _context.Groups.Remove(groups);
            await _context.SaveChangesAsync();
            return RedirectToAction("DepartmentPage","Departments", new { id = DepId}) ;
        }

        public IActionResult Excel(int DepId)
        {
            Departments departments =  _context.Departments.Find(DepId);
            departments.Groups = _context.Groups.ToList();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Groups");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Parameters";
                foreach (Groups groups in departments.Groups)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = groups.id;
                    worksheet.Cell(currentRow, 2).Value = groups.Name;
                    string Parameters = " ";
                    if (groups.DepartmentId == DepId)
                    {
                        foreach (ParametersGroup groupsParameters in _context.ParametersGroups.ToList())
                        {
                            if (groupsParameters.GroupId == groups.id)
                            {
                                foreach (Parameters parameters in _context.Parameters.ToList())
                                {
                                    if (parameters.Id == groupsParameters.ParameterId)
                                    {
                                        Parameters = Parameters  + parameters.Name + "\n";

                                    }
                                }
                            }
                        }
                    }
                    worksheet.Cell(currentRow, 3).Value = Parameters;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Groups.xlsx");
                }
            }
        }

    }
}
