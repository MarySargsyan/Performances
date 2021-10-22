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
using Perfomans.ViewModels;

namespace Perfomans.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationContext _context;

        public DepartmentsController(ApplicationContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View(_context.Departments.ToList());
        }
         public ActionResult GroupsIndex()
        {
            return View(_context.Groups.ToList());
        }

        public ActionResult DepartmentPage(int? id)
        {
            List<Evaluations> lastevaluations = new List<Evaluations>();
            foreach (Evaluations evaluations in _context.Evaluations.ToList())
            {
                lastevaluations.Add(evaluations);
            }
            foreach (Evaluations laste in lastevaluations.ToList())
            { 
                foreach (Evaluations e in _context.Evaluations.ToList())
                {
                        if ((e.UserId == laste.UserId) & (e.ParameterId == laste.ParameterId) & (e.Id < laste.Id))
                        {
                            lastevaluations.Remove(e);
                        }
                }
            }

            ViewBag.Evaluations = lastevaluations;
            ViewBag.Parameters = _context.Parameters.ToList();
            ViewBag.ParametersGroups = _context.ParametersGroups.ToList();
            ViewBag.Heads = _context.User.ToList();
            Departments departments = _context.Departments.Find(id);
            departments.DepartmentParameters = _context.DepartmentParameters.ToList();
            departments.Head = _context.Heads.ToList();
            departments.Groups = _context.Groups.ToList();
            departments.User = _context.User.ToList();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departments);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departments = await _context.Departments.FindAsync(id);
            if (departments == null)
            {
                return NotFound();
            }
            return View(departments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Departments departments)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentsExists(departments.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(departments);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var departments = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(departments);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departments = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(departments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult EmployeePartial()
        {
            return PartialView();
        }
        public ActionResult ParametersPartial()
        {
            ViewBag.DepartmentParameters = _context.DepartmentParameters.ToList();
            ViewBag.Parameters = _context.Parameters.ToList();
            return PartialView();
        }
        public ActionResult GroupsPartial()
        {
            ViewBag.Parameters = _context.Parameters.ToList();
            ViewBag.Groups = _context.Groups.ToList();
            return PartialView();
        }
        private bool DepartmentsExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
