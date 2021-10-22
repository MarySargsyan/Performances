using Microsoft.AspNetCore.Mvc;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Controllers
{
    public class GroupsParameters : Controller
    {
        private readonly ApplicationContext _context;

        public GroupsParameters(ApplicationContext context)
        {
            _context = context;
        }

        public ActionResult Edit(int ParametertId, int GroupId)
        {
            foreach (ParametersGroup groupsParameters in _context.ParametersGroups.ToList())
            {
                if ((groupsParameters.GroupId == GroupId) & (groupsParameters.ParameterId == ParametertId))
                {
                    return View(groupsParameters);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id, ParameterId, GroupId, Mark")] ParametersGroup parametersGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Update(parametersGroup);
                _context.SaveChanges();
                return RedirectToAction("EditGroupsParam", "Groups", new { id = parametersGroup.GroupId });
            }
            return View(parametersGroup);
        }

    }
}
