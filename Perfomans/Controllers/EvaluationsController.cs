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
        public int GetCorrentUserId()
        {
            int CorrentUserId = 0;
            foreach (User user in _context.User.ToList())
            {
                if (User.Identity.Name == user.Email)
                {
                    CorrentUserId = user.Id;
                }
            }
            return (CorrentUserId);
        }
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Evaluations.Where(e=> e.AssessorId == GetCorrentUserId()||
            e.Assessor.SupervisorId == GetCorrentUserId())
                .Include(u => u.Assessor).
                Include(u => u.Parameter).
                Include(u => u.User);
            return View(await applicationContext.ToListAsync());
        }

        public IActionResult Create()
        {
            List<User> myEmployees = new List<User>();

                foreach (User users in _context.User.Where(u =>u.SupervisorId == GetCorrentUserId()).ToList())
                { 
                    myEmployees.Add(users);
                    foreach (User u in _context.User.Where(u=>u.SupervisorId == users.Id).ToList())
                    {
                       myEmployees.Add(u);
                    }
                }

            ViewData["ParameterId"] = new SelectList(_context.Parameters, "Id", "Name", _context.Parameters.Select(x => x.Id).FirstOrDefault());
            ViewData["UserId"] = new SelectList(myEmployees, "Id", "Name", _context.User.Select(x => x.Id).FirstOrDefault());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,ParameterId,UserId,Mark")] Evaluations evaluations)
        {
            if (ModelState.IsValid)
            {
                evaluations.AssessorId = GetCorrentUserId();
                _context.Add(evaluations);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evaluation = await _context.Evaluations.FindAsync(id);
            _context.Evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
