using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SourName { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public int? StateId { get; set; }
        public State state { get; set; }
        
        public int? SupervisorId { get; set; }
        public User Supervisor { get; set; }
        
        public int? DepartmentId { get; set; }
        public Departments Department { get; set; }

        public List<DepartmentHead> DeportamentHead { get; set; }
        [NotMapped]
        public double result { get; set; }
        [NotMapped]
        public int progress { get; set; }
    }
}
