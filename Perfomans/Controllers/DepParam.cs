using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Perfomans.Models;
using Perfomans.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Controllers
{
    public class DepParam : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IDepParamService _service;

        public DepParam(ApplicationContext context, IDepParamService service)
        {
            _context = context;
            _service = service;
        }

        public ActionResult Create(int depId)
        {
            ViewBag.DepId = depId;
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", _context.Parameters.Select(x => x.Id).FirstOrDefault());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ParameterId, DepartmentId, mark")] DepartmentParameters departmentParameters, int? DepId)
        {
            if (ModelState.IsValid)
            {
                _service.Insert(departmentParameters);
            }
            return RedirectToAction("DepartmentPage", "Departments", new {id = departmentParameters.DepartmentId });

        }

        public ActionResult Edit(int DepartmentId, int ParameterId)
        {
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", _context.Parameters.Select(x => x.Id).FirstOrDefault());
            return View(_service.GetById(DepartmentId, ParameterId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ParameterId, DepartmentId, mark")] DepartmentParameters departmentParameters)
        {
            _service.Update(departmentParameters);
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", _context.Parameters.Select(x => x.Id).FirstOrDefault());
            return RedirectToAction("DepartmentPage", "Departments", new { id = departmentParameters.DepartmentId });
        }

        public ActionResult Delete(int DepartmentId, int ParameterId)
        {
            ViewBag.ParameterName = _context.Parameters.Find(ParameterId).Name;
            return View(_service.GetById(DepartmentId, ParameterId));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int DepartmentId, int ParameterId)
        {
            _service.Delete(DepartmentId, ParameterId);
            return RedirectToAction("DepartmentPage", "Departments", new { id = DepartmentId });
        }
    }
}
