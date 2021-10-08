using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> User { get; set; }
        public State()
        {
            User = new List<User>();
        }
    }
}
