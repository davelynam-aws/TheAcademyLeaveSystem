using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace The_Academy_Leave_System.Models
{
    /// <summary>
    /// This static class holds the variables for the current logged in user.
    /// </summary>
    public static class CurrentUser
    {
        public static int Id { get; set; }

        //public static int TeamId { get; set; }
 
        //public static int RoleId { get; set; }
    
        public static string FirstName { get; set; }
  
        public static string LastName { get; set; }

        //public static bool IsActive { get; set; }
      
        //public static decimal LeaveAllowanceThisYear { get; set; }

        //public static decimal LeaveAllowanceNextYear { get; set; }
 
        public static string Email { get; set; }

        //public static string PasswordHash { get; set; }

        //public static DateTime LastLoggedInDateTime { get; set; }

        //public static string Password { get; set; }
    }
}
