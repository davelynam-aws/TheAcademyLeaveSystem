using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace The_Academy_Leave_System.Models
{

    public class ResetPassword
    {

 
        public int Id { get; set; }

        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The confirmed password doesn't match, please try again.")]
        public string ConfirmPassword { get; set; }

    }
}
