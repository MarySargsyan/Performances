using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class Groups
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int? DepartmentId { get; set; }
        public Departments Department { get; set; }

        public List<ParametersGroup> ParametersGroups { get; set; }

    }
}
