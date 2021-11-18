using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using Perfomans.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Controllers
{
    public class EvaluationsController : Controller
    {
        private readonly IEvalService _service;
        private readonly ApplicationContext _context;

        public EvaluationsController(IEvalService service, ApplicationContext context)
        {
            _service = service;
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_service.AllCorrentAssesorEvaluations(User.Identity.Name));
        }
        public IActionResult EvaluationPage(int? id)
        {
            ViewBag.Parameters = _service.AllParam();
            ViewBag.AllUsers = _service.MyEmployees(User.Identity.Name);
            var evaluation = _service.GetById(id);
            return View(evaluation);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Date,ParameterId,UserId,Mark")] Evaluations evaluations)
        {
            if (ModelState.IsValid)
            {
                evaluations.AssessorId = _service.GetCorrentAssesor(User.Identity.Name);
                _service.Insert(evaluations);
                return RedirectToAction("EvaluationMarksGrid", new { id = evaluations.Id });
            }

            return View(evaluations);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,Date,ParameterId,UserId,Mark")] Evaluations evaluations, List<int> marks)
        {
            _service.Update(evaluations, marks);
            return RedirectToAction("EvaluationPage", new { id = evaluations.Id});
        }

        public IActionResult EvaluationMarksGrid(int? id)
        {
            ViewBag.Parameters = _service.AllParam();
            ViewBag.AllUsers = _service.MyEmployees(User.Identity.Name);
            var evaluation = _service.GetById(id);
            return View(evaluation);
        }

        public IActionResult Delete(int? id)
        {
            var evaluation = _service.GetById(id); 
            return View(evaluation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
