using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Academy_Leave_System.ViewModels;

namespace The_Academy_Leave_System.Models
{
    public static class GlobalClashes
    {
        public static List<LeaveRequestViewModel> Clashes { get; set; } = new List<LeaveRequestViewModel>();
    }
}
