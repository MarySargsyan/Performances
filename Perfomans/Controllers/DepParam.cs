using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Controllers
{
    public class DepParam : Controller
    {
        private readonly ApplicationContext _context;

        public DepParam(ApplicationContext context)
        {
            _context = context;
        }

        public ActionResult Create(int depId)
        {
            ViewBag.DepId = depId;
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", _context.Parameters.Select(x => x.Id).FirstOrDefault());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id, ParameterId, DepartmentId, mark")] DepartmentParameters departmentParameters, int? DepId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentParameters);
                _context.SaveChanges();
            }
            return RedirectToAction("DepartmentPage", "Departments", new {id = departmentParameters.DepartmentId });

        }

        public ActionResult Edit(int DepartmentId, int ParameterId)
        {
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", _context.Parameters.Select(x => x.Id).FirstOrDefault());
            foreach (DepartmentParameters department in _context.DepartmentParameters)
            {
                if ((department.DepartmentId == DepartmentId) & (department.ParameterId == ParameterId))
                {
                    return View(department);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id, ParameterId, DepartmentId, mark")] DepartmentParameters departmentParameters)
        {
            _context.Update(departmentParameters);
             _context.SaveChanges();
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", _context.Parameters.Select(x => x.Id).FirstOrDefault());
            return RedirectToAction("DepartmentPage", "Departments", new { id = departmentParameters.DepartmentId });
        }

        public ActionResult Delete(int DepartmentId, int ParameterId)
        {
            ViewBag.ParameterName = _context.Parameters.Find(ParameterId).Name;
            foreach (DepartmentParameters department in _context.DepartmentParameters)
            {
                if ((department.DepartmentId == DepartmentId) & (department.ParameterId == ParameterId))
                {
                  return View(department);
                }
            }
            return View();

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int DepartmentId, int ParameterId)
        {
           foreach(DepartmentParameters department in _context.DepartmentParameters)
           {
                if ((department.DepartmentId == DepartmentId) & (department.ParameterId == ParameterId))
                {
                    _context.DepartmentParameters.Remove(department);
                }
           }
            _context.SaveChanges();
            return RedirectToAction("DepartmentPage", "Departments", new { id = DepartmentId });

        }
    }
}
