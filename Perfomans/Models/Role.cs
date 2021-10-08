using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> User { get; set; }
        public Role()
        {
            User = new List<User>();
        }
    }
}
