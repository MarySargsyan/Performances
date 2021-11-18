using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIMethod.Models;

namespace WebAPIMethod.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
       GroupsContext _context;

        public ValuesController(GroupsContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Groups>>> Get()
        {
            //Departments departments = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
            //if (departments == null)
            //    return NotFound();
            //var selectedGroups = from t in _context.Groups
            //                    where t.DepartmentId == id
            //                    select t; 
            //return await selectedGroups.ToListAsync();
            return await _context.Groups.ToListAsync();

        }
    }
}
