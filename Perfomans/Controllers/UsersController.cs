using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using Perfomans.Service;

namespace Perfomans.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IUserService _service;

        public UsersController(ApplicationContext context, IUserService service)
        {
            _context = context;
            _service = service;
        }
        [Authorize(Roles = "admin")]

        public IActionResult Index()
        {
            var users = _service.AllUsers();
            return View(users);
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
        public IActionResult Create([Bind("Id,Name,SourName,Email,Password,RoleId,StateId,SupervisorId,DepartmentId")] User user)
        {
            if (ModelState.IsValid)
            {
                _service.Insert(user);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            ViewData["SupervisorId"] = new SelectList(_context.User, "Id", "Name", user.SupervisorId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", user.StateId);
            return View(user);
        }

        public IActionResult Edit(int? id)
        {
          User user = _service.GetById(id);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", _context.Departments.Select(x => x.Id).FirstOrDefault());
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", _context.Roles.Select(x => x.Id).FirstOrDefault());
            ViewData["SupervisorId"] = new SelectList(_context.User, "Id", "Name", _context.User.Select(x => x.Id).FirstOrDefault());
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", _context.States.Select(x => x.Id).FirstOrDefault());
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,SourName,Email,Password,RoleId,StateId,SupervisorId,DepartmentId")] User user)
        {
            _service.Update(user);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            ViewData["SupervisorId"] = new SelectList(_context.User, "Id", "Name", user.SupervisorId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", user.StateId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            var user = _service.GetUserNmodelsById(id);
            return View(user);
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
