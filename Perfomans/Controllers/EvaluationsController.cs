using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Controllers
{
    public class EvaluationsController : Controller
    {
        private readonly ApplicationContext _context;

        public EvaluationsController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Evaluations.Include(u => u.Assessor).Include(u => u.Parameter).Include(u => u.User);
            return View(await applicationContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", _context.Parameters.Select(x => x.Id).FirstOrDefault());
            ViewData["AssessorId"] = new SelectList(_context.User, "Id", "Name", _context.User.Select(x => x.Id).FirstOrDefault());
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", _context.User.Select(x => x.Id).FirstOrDefault());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Date,ParameterId,AssessotId,UserId,Mark")] Evaluations evaluations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evaluations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", evaluations.ParameterId);
            ViewData["AssessorId"] = new SelectList(_context.User, "Id", "Name", evaluations.AssessorId);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evaluation = await _context.Evaluations.FindAsync(id);
            _context.Evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
