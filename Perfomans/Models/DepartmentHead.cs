using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class DepartmentHead
    {
        public int Id { get; set; }
        public int? HeadId { get; set; }
        public User Head { get; set; }
        public int? DepartmentId { get; set; }
        public Departments Departments { get; set; }
    }
}
