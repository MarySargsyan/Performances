using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIMethod.Models
{
    public class Parameters
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ParametersGroup> ParametersGroups { get; set; }


    }
}
