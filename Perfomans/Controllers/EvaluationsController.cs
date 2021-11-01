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

        public IActionResult Create()
        {
            List<User> myEmployees = _service.MyEmployees(User.Identity.Name);
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", _context.Parameters.Select(x => x.Id).FirstOrDefault());
            ViewData["UserId"] = new SelectList(myEmployees, "Id", "Name", _context.User.Select(x => x.Id).FirstOrDefault());
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", evaluations.ParameterId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", evaluations.UserId);
            return View(evaluations);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var evaluation = await _context.Evaluations.Include(u => u.Parameter).Include(u => u.User).Include(u => u.Assessor).FirstOrDefaultAsync(m => m.Id == id);
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
