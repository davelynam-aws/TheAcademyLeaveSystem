using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using The_Academy_Leave_System.Models;

namespace The_Academy_Leave_System.ViewModels
{
    public class LeaveRequestViewModel
    {
        public LeaveRequest ThisLeaveRequest { get; set; }

        public User ThisUser { get; set; }

        [Display(Name = "AM/PM")]
        public List<SelectListItem> HalfDayIdentifierOptions = new List<SelectListItem>()
        {          
            new SelectListItem() { Text="AM", Value="AM"},
            new SelectListItem() { Text="PM", Value="PM"},
            new SelectListItem() { Text="N/A", Value="N/A"},
        };

        
        public string Status { get; set; }

        public string FullName { get; set; }

        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();

        public int SelectedUserId { get; set; }

    }
}
