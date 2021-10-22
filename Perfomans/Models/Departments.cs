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
        public List<DepartmentParameters> DepartmentParameters { get; set; }
        public List<DepartmentHead> Head { get; set; }

        public List<User> User { get; set; }
        public List<Groups> Groups { get; set; }
        public Departments()
        {
            User = new List<User>();
            Groups = new List<Groups>();
        }
    }
}
