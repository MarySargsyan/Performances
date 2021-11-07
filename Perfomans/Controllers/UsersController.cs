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
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
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
            var Users = _service.AllUsers();
            var Depo = _service.AllDepartments();
            var Role = _service.AllRoles();
            var State = _service.AllStates();

            ViewData["SupervisorId"] = new SelectList(Users.ToList(), "Id", "Name", Users.ToList().Select(x => x.Id).FirstOrDefault());
            ViewData["DepartmentId"] = new SelectList(Depo.ToList(), "Id", "Name", Depo.ToList().Select(x => x.Id).FirstOrDefault());
            ViewData["RoleId"] = new SelectList(Role.ToList(), "Id", "Name", Role.ToList().Select(x => x.Id).FirstOrDefault());
            ViewData["StateId"] = new SelectList(State.ToList(), "Id", "Name", State.ToList().Select(x => x.Id).FirstOrDefault());
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
            var Users = _service.AllUsers();
            var Depo = _service.AllDepartments();
            var Role = _service.AllRoles();
            var State = _service.AllStates();

            ViewData["DepartmentId"] = new SelectList(Depo.ToList(), "Id", "Name", user.DepartmentId);
            ViewData["RoleId"] = new SelectList(Role.ToList(), "Id", "Name", user.RoleId);
            ViewData["SupervisorId"] = new SelectList(Users.ToList(), "Id", "Name", user.SupervisorId);
            ViewData["StateId"] = new SelectList(State.ToList(), "Id", "Name", user.StateId);
            return View(user);
        }

        public IActionResult Edit(int? id)
        {
          User user = _service.GetById(id);
            var Users = _service.AllUsers();
            var Depo = _service.AllDepartments();
            var Role = _service.AllRoles();
            var State = _service.AllStates();

            ViewData["SupervisorId"] = new SelectList(Users.ToList(), "Id", "Name", Users.ToList().Select(x => x.Id).FirstOrDefault());
            ViewData["DepartmentId"] = new SelectList(Depo.ToList(), "Id", "Name", Depo.ToList().Select(x => x.Id).FirstOrDefault());
            ViewData["RoleId"] = new SelectList(Role.ToList(), "Id", "Name", Role.ToList().Select(x => x.Id).FirstOrDefault());
            ViewData["StateId"] = new SelectList(State.ToList(), "Id", "Name", State.ToList().Select(x => x.Id).FirstOrDefault());
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,SourName,Email,Password,RoleId,StateId,SupervisorId,DepartmentId")] User user)
        {
            _service.Update(user);
            var Users = _service.AllUsers();
            var Depo = _service.AllDepartments();
            var Role = _service.AllRoles();
            var State = _service.AllStates();

            ViewData["DepartmentId"] = new SelectList(Depo.ToList(), "Id", "Name", user.DepartmentId);
            ViewData["RoleId"] = new SelectList(Role.ToList(), "Id", "Name", user.RoleId);
            ViewData["SupervisorId"] = new SelectList(Users.ToList(), "Id", "Name", user.SupervisorId);
            ViewData["StateId"] = new SelectList(State.ToList(), "Id", "Name", user.StateId);
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
