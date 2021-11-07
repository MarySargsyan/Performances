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
        private readonly IDepParamService _service;

        public DepParam(IDepParamService service)
        {
            _service = service;
        }

        public ActionResult Create(int depId)
        {
            ViewBag.DepId = depId;
            var Parameters = _service.AllParameters();
            ViewData["ParameterId"] = new SelectList(Parameters.ToList(), "Id", "Name", Parameters.ToList().Select(x => x.Id).FirstOrDefault());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ParameterId, DepartmentId, mark")] DepartmentParameters departmentParameters)
        {
            if (ModelState.IsValid)
            {
                _service.Insert(departmentParameters);
            }
            return RedirectToAction("DepartmentPage", "Departments", new {id = departmentParameters.DepartmentId });

        }

        public ActionResult Edit(int DepartmentId, int ParameterId)
        {
            var Parameters = _service.AllParameters();
            ViewData["ParameterId"] = new SelectList(Parameters.ToList(), "Id", "Name", Parameters.ToList().Select(x => x.Id).FirstOrDefault());
            return View(_service.GetById(DepartmentId, ParameterId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ParameterId, DepartmentId, mark")] DepartmentParameters departmentParameters)
        {
            _service.Update(departmentParameters);
            var Parameters = _service.AllParameters();

            ViewData["ParameterId"] = new SelectList(Parameters.ToList(), "Id", "Name", Parameters.ToList().Select(x => x.Id).FirstOrDefault());
            return RedirectToAction("DepartmentPage", "Departments", new { id = departmentParameters.DepartmentId });
        }

        public ActionResult Delete(int DepartmentId, int ParameterId)
        {
            var Parameters = _service.AllParameters();
            ViewBag.ParameterName = Parameters.ToList().Where(p => p.Id == ParameterId).Select(p => p.Name);
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
