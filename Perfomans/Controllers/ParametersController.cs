using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;

namespace Perfomans.Controllers
{
    public class ParametersController : Controller
    {
        private readonly ApplicationContext _context;

        public ParametersController(ApplicationContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin")]

        // GET: Parameters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parameters.ToListAsync());
        }

        // GET: Parameters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameters = await _context.Parameters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parameters == null)
            {
                return NotFound();
            }

            return View(parameters);
        }

        // GET: Parameters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parameters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Coefficient,Mark_1_description,Mark_2_description,Mark_3_description,Mark_4_description,Mark_5_description")] Parameters parameters)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parameters);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parameters);
        }

        // GET: Parameters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameters = await _context.Parameters.FindAsync(id);
            if (parameters == null)
            {
                return NotFound();
            }
            return View(parameters);
        }

        // POST: Parameters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Coefficient,Mark_1_description,Mark_2_description,Mark_3_description,Mark_4_description,Mark_5_description")] Parameters parameters)
        {

            if (ModelState.IsValid)
            {  
                _context.Update(parameters);
                await _context.SaveChangesAsync();           
                return RedirectToAction(nameof(Index));
            }
            return View(parameters);
        }

        // GET: Parameters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var parameters = await _context.Parameters
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(parameters);
        }

        // POST: Parameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parameters = await _context.Parameters.FindAsync(id);
            _context.Parameters.Remove(parameters);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParametersExists(int id)
        {
            return _context.Parameters.Any(e => e.Id == id);
        }
    }
}
