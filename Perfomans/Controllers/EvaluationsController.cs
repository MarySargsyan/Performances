using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using Perfomans.Service;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

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
                _context.Evaluations.Add(evaluations);
                _context.SaveChanges();
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
        [HttpPost]
        public IActionResult Import(IFormFile file, int? id)
        {
           List<UserParamEval> upelist = _context.UserParamEval.Where(upe=> upe.EvaluationsId == id).ToList();
            using (var stream = new MemoryStream())
            {
                file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    var colscount = worksheet.Dimension.Columns;
                    for(int row = 2; row<= rowcount; row++)
                    {
                        for(int coll = 2; coll<= colscount; coll++)
                        {
                           foreach(UserParamEval paramEval in upelist)
                            {
                             paramEval.Mark = Convert.ToInt32(worksheet.Cells[row,coll].Value.ToString().Trim());

                            }
                        }
                    }
                } 
            }
            _context.SaveChanges();
            return RedirectToAction("EvaluationPage", new { id= id});
        }

    }
}
