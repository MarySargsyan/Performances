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
    public class UsersController : Controller
    {
        private readonly ApplicationContext _context;

        public UsersController(ApplicationContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.User.Include(u => u.Department).Include(u => u.Role).Include(u => u.Supervisor).Include(u => u.state);
            return View(await applicationContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", _context.Departments.Select(x => x.Id).FirstOrDefault());
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", _context.Roles.Select(x => x.Id).FirstOrDefault());
            ViewData["SupervisorId"] = new SelectList(_context.User, "Id", "Name", _context.User.Select(x => x.Id).FirstOrDefault());
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", _context.States.Select(x => x.Id).FirstOrDefault());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SourName,Email,Password,RoleId,StateId,SupervisorId,DepartmentId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            ViewData["SupervisorId"] = new SelectList(_context.User, "Id", "Name", user.SupervisorId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", user.StateId);
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", _context.Departments.Select(x => x.Id).FirstOrDefault());
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", _context.Roles.Select(x => x.Id).FirstOrDefault());
            ViewData["SupervisorId"] = new SelectList(_context.User, "Id", "Name", _context.User.Select(x => x.Id).FirstOrDefault());
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", _context.States.Select(x => x.Id).FirstOrDefault());
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SourName,Email,Password,RoleId,StateId,SupervisorId,DepartmentId")] User user)
        {
             _context.Update(user);
             await _context.SaveChangesAsync();
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            ViewData["SupervisorId"] = new SelectList(_context.User, "Id", "Name", user.SupervisorId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", user.StateId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _context.User.Include(u => u.Department).Include(u => u.Role).Include(u => u.Supervisor).Include(u => u.state).FirstOrDefaultAsync(m => m.Id == id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
