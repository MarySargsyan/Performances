using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class Departments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DepartmentParameters> Parameters { get; set; }
    }
}
