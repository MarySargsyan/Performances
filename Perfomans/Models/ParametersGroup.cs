using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class ParametersGroup
    {
        public int? ParameterId { get; set; }
        public Parameters Parameters { get; set; }
        public int? GroupId { get; set; }
        public Groups Groups { get; set; }
        public double Mark { get; set; }


    }
}
