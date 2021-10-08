using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class DepartmentParameters
    {
        public int? DepartmentId { get; set; }
        public Departments Department { get; set; }
        public int? ParameterId { get; set; }
        public Parameters parameter { get; set; }
        public double mark { get; set; }


    }
}
