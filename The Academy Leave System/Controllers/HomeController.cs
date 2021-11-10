using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using The_Academy_Leave_System.Methods;
using The_Academy_Leave_System.Models;
using The_Academy_Leave_System.ViewModels;

namespace The_Academy_Leave_System.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Register db context for TALS database.
        private readonly DBContext _context;
        

        public HomeController(ILogger<HomeController> logger, DBContext dBContext)
        {
            _logger = logger;
            _context = dBContext;
        }

        public IActionResult Index()
        {
            UserViewModel userViewModel = new UserViewModel();

            // If the user is not logged in then always redirect to the login page.
            // This should be present on each page to prevent unauthorised access and errors.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");   
            }

       
            DateTime startOfNextYear = DateTime.Parse($"01/01/{DateTime.Now.AddYears(1).Year}");

            userViewModel.ThisUser = _context.Users.Where(u => u.Id == CurrentUser.Id).Single();
            userViewModel.MyLeaveRequests = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id).ToList();

            userViewModel.LeaveAwaitingApprovalThisYear = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id && l.ApprovedDateTime == DateTime.Parse("01/01/1753") && l.RejectedDateTime == DateTime.Parse("01/01/1753") && l.RequestedLeaveEndDate < startOfNextYear).Select(l => l.TotalDaysRequested).Sum();

            userViewModel.LeaveAwaitingApprovalNextYear = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id && l.ApprovedDateTime == DateTime.Parse("01/01/1753") && l.RejectedDateTime == DateTime.Parse("01/01/1753") && l.RequestedLeaveStartDate >= startOfNextYear).Select(l => l.TotalDaysRequested).Sum();

            userViewModel.LeaveBookedThisYear = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id && l.ApprovedDateTime != DateTime.Parse("01/01/1753") && l.RejectedDateTime == DateTime.Parse("01/01/1753") && l.RequestedLeaveEndDate < startOfNextYear).Select(l => l.TotalDaysRequested).Sum();

            userViewModel.LeaveBookedNextYear = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id && l.ApprovedDateTime == DateTime.Parse("01/01/1753") && l.RejectedDateTime == DateTime.Parse("01/01/1753") && l.RequestedLeaveStartDate >= startOfNextYear).Select(l => l.TotalDaysRequested).Sum();

            userViewModel.LeaveLeftThisYear = userViewModel.ThisUser.LeaveAllowanceThisYear - userViewModel.LeaveBookedThisYear;

            userViewModel.LeaveLeftNextYear = userViewModel.ThisUser.LeaveAllowanceNextYear - userViewModel.LeaveBookedNextYear;

            return View(userViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
