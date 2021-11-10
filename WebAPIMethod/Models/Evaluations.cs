using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIMethod.Models
{
    public class Evaluations
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int? AssessorId { get; set; }
        public User Assessor { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int? ParameterId { get; set; }
        public Parameters Parameter { get; set; }

        public double Mark { get; set; }

    }
}
