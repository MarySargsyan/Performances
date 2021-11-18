using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIMethod.Models
{
    public class UserParamEval
    {
        public int? EvaluationId { get; set; }
        public Evaluations Evaluations { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }

        public int? ParameterId { get; set; }
        public Parameters Parameter { get; set; }

        public double Mark { get; set; }
    }
}
