using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
