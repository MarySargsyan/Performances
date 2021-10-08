using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Perfomans.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "admin, user")]
        public IActionResult Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (role == "admin")
            {
                return RedirectToAction("Index", "Parameters");
            }
            else
            {
                return Content($"employee");

            }
        }
    }
}
