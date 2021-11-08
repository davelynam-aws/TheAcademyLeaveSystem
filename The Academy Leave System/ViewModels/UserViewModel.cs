using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using The_Academy_Leave_System.Models;

namespace The_Academy_Leave_System.ViewModels
{
    public class UserViewModel
    {
        public User ThisUser { get; set; }

        [Display(Name = "Team")]
        public List<SelectListItem> TeamOptions = new List<SelectListItem>();

        [Display(Name = "Role")]
        public List<SelectListItem> RoleOptions = new List<SelectListItem>();

        //[Required]
        //public string Password { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }




        // Constructor to create a new user object.
        public UserViewModel()
        {
            ThisUser = new User();
        }



    }
}
