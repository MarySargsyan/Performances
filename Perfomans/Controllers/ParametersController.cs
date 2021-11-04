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
    public class ParametersController : Controller
    {
        private readonly IParametersService _service;

        public ParametersController(IParametersService service)
        {
            _service = service;
        }
        [Authorize(Roles = "admin")]

        public IActionResult Index()
        {
            var parameters = _service.AllParameters();
            return View(parameters);
        }

        public IActionResult Details(int? id)=> View(_service.GetById(id));
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Coefficient,Mark_1_description,Mark_2_description,Mark_3_description,Mark_4_description,Mark_5_description")] Parameters parameters)
        {
            if (ModelState.IsValid)
            {
                _service.Insert(parameters);
                return RedirectToAction(nameof(Index));
            }
            return View(parameters);
        }
        public IActionResult Edit(int? id)=> View(_service.GetById(id));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Coefficient,Mark_1_description,Mark_2_description,Mark_3_description,Mark_4_description,Mark_5_description")] Parameters parameters)
        {
            if (ModelState.IsValid)
            {
                _service.Update(parameters);
                return RedirectToAction(nameof(Index));
            }
            return View(parameters);
        }
        public IActionResult Delete(int? id)=> View(_service.GetById(id));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
