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


        // Historical Leave Requests for this user.
        public List<LeaveRequest> MyLeaveRequests { get; set; }

        // Historical Event logs for this user.
        public List<EventLog> MyEventLogs { get; set; }

        // Newly created leave request
        public LeaveRequest ThisLeaveRequest { get; set; }


        // Constructor to create a new user object.
        public UserViewModel()
        {
            ThisUser = new User();
        }




        public decimal LeaveLeftThisYear { get; set; }
        public decimal LeaveLeftNextYear { get; set; }
        public decimal LeaveAwaitingApprovalThisYear { get; set; }
        public decimal LeaveAwaitingApprovalNextYear { get; set; }
        public decimal LeaveBookedThisYear { get; set; }
        public decimal LeaveBookedNextYear { get; set; }


    }
}
