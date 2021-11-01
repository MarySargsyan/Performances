using Microsoft.AspNetCore.Mvc;
using Perfomans.Models;
using Perfomans.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Controllers
{
    public class GroupsParameters : Controller
    {
        private readonly IGroupService _service;
        public GroupsParameters(IGroupService service)
        {
            _service = service;
        }
        public ActionResult Edit(int ParametertId, int GroupId)
        {
            ParametersGroup groupsParameters = _service.GetGroupParam(ParametertId, GroupId);
            return View(groupsParameters);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id, ParameterId, GroupId, Mark")] ParametersGroup parametersGroup)
        {
            if (ModelState.IsValid)
            {
                _service.EditParametersGroup(parametersGroup);
                return RedirectToAction("EditGroupsParam", "Groups", new { id = parametersGroup.GroupId });
            }
            return View(parametersGroup);
        }
    }
}
