using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class Parameters
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Coefficient { get; set; }
        public string Mark_1_description { get; set; }
        public string Mark_2_description { get; set; }
        public string Mark_3_description { get; set; }
        public string Mark_4_description { get; set; }
        public string Mark_5_description { get; set; }

        public List<DepartmentParameters> Departments { get; set; }
        public List<Evaluations> evaluations { get; set; }
        public Parameters()
        {
            evaluations = new List<Evaluations>();
        }

    }
}
