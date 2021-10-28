using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Grpc.Core;
using System.Drawing.Printing;
using System.Drawing.Text;
using Perfomans.Models;

namespace Perfomans.Controllers
{
    public class ChartController : Controller
    {
        private readonly ApplicationContext _context;

        public ChartController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.AllUsers = _context.User.ToList();
            ViewBag.Head = _context.Heads.ToList();
            return View(_context.Departments.ToList());
        }
    }

}
