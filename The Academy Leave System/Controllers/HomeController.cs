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
 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Register db context for TALS database.
        private readonly DBContext _context;
        

        public HomeController(ILogger<HomeController> logger, DBContext dBContext)
        {
            // Assign db conetext and logger.
            _logger = logger;
            _context = dBContext;
        }

        public IActionResult Index()
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Create new viewmodel.
            UserViewModel userViewModel = new UserViewModel();

            // If the user is not logged in then always redirect to the login page.
            // This should be present on each page to prevent unauthorised access and errors.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");   
            }

            // Set comparisson dates for identifying this year and next year leave.
            DateTime startOfNextYear = DateTime.Parse($"01/01/{DateTime.Now.AddYears(1).Year}");
            DateTime nullDateTime = DateTime.Parse("01/01/1753");


            // This section gets all aggregate leave data for the user.
            userViewModel.ThisUser = _context.Users.Where(u => u.Id == CurrentUser.Id).Single();
            userViewModel.MyLeaveRequests = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id).ToList();

            userViewModel.LeaveAwaitingApprovalThisYear = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id && l.ApprovedDateTime == nullDateTime && l.RejectedDateTime == nullDateTime && l.RequestedLeaveEndDate < startOfNextYear && l.IsCancelled == false).Select(l => l.TotalDaysRequested).Sum();

            userViewModel.LeaveAwaitingApprovalNextYear = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id && l.ApprovedDateTime == nullDateTime && l.RejectedDateTime == nullDateTime && l.RequestedLeaveStartDate >= startOfNextYear && l.IsCancelled == false).Select(l => l.TotalDaysRequested).Sum();

            userViewModel.LeaveBookedThisYear = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id && l.ApprovedDateTime != nullDateTime && l.RejectedDateTime == nullDateTime && l.RequestedLeaveEndDate < startOfNextYear && l.IsCancelled == false).Select(l => l.TotalDaysRequested).Sum();

            userViewModel.LeaveBookedNextYear = _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id && l.ApprovedDateTime == nullDateTime && l.RejectedDateTime == nullDateTime && l.RequestedLeaveStartDate >= startOfNextYear && l.IsCancelled == false).Select(l => l.TotalDaysRequested).Sum();

            userViewModel.LeaveLeftThisYear = userViewModel.ThisUser.LeaveAllowanceThisYear - userViewModel.LeaveBookedThisYear - userViewModel.LeaveAwaitingApprovalThisYear;

            userViewModel.LeaveLeftNextYear = userViewModel.ThisUser.LeaveAllowanceNextYear - userViewModel.LeaveBookedNextYear - userViewModel.LeaveAwaitingApprovalNextYear;



            // Get all unseen leave request notifications and send to the view to be displayed by the designer as a notification.
            List<LeaveRequest> leaveRequestNotifications = new List<LeaveRequest>();

            List<string> notificationMessage = new List<string>();

            // Get this user's notifications
            leaveRequestNotifications = _context.LeaveRequests.Where(lr => lr.UserId == CurrentUser.Id && lr.UserNotified == false && (lr.ApprovedDateTime > nullDateTime || lr.ApprovedDateTime > nullDateTime)).ToList();

            if (leaveRequestNotifications.Count > 0)
            {
                foreach(LeaveRequest lr in leaveRequestNotifications)
                {
                    if (lr.ApprovedDateTime > nullDateTime)
                    {
                        notificationMessage.Add($"Leave denied for {Convert.ToDateTime(lr.RequestedLeaveStartDate).ToShortDateString()} to {Convert.ToDateTime(lr.RequestedLeaveEndDate).ToShortDateString()}");
                    }
                    else
                    {
                        notificationMessage.Add($"Leave denied for {Convert.ToDateTime(lr.RequestedLeaveStartDate).ToShortDateString()} to {Convert.ToDateTime(lr.RequestedLeaveEndDate).ToShortDateString()}");
                    }

                    // Update the leave request so the user is only notified once.
                    lr.UserNotified = true;
                    _context.Update(lr);
                    _context.SaveChangesAsync();

                }
            }

            // If the user is a supervisor then get all pending requests and display as a notification until they are actioned.
            if(CurrentUser.Role == "Supervisor")
            {
                List<User> teamUsers = new List<User>();
                teamUsers = _context.Users.Where(u => u.TeamId == userViewModel.ThisUser.TeamId && u.Id != CurrentUser.Id).ToList();

                if (teamUsers.Count > 0)
                {
                    foreach(User u in teamUsers)
                    {

                        leaveRequestNotifications = _context.LeaveRequests.Where(lr => lr.UserId == u.Id && lr.ManagerNotified == false).ToList();

                        if (leaveRequestNotifications.Count > 0)
                        {
                            foreach (LeaveRequest lr in leaveRequestNotifications)
                            {

                                notificationMessage.Add($"Leave requested by {u.FirstName} {u.LastName} from {Convert.ToDateTime(lr.RequestedLeaveStartDate).ToShortDateString()} to {Convert.ToDateTime(lr.RequestedLeaveEndDate).ToShortDateString()}");
                                
                            }
                        }
                    }
                }

            }


            // If the user is a manager then get all pending requests and display as a notification until they are actioned.
            if (CurrentUser.Role == "Manager")
            {
                List<User> supervisorUsers = new List<User>();

                int supervisorRoleId = _context.Roles.Where(r => r.RoleName == "Supervisor").Select(r => r.Id).Single();

                supervisorUsers = _context.Users.Where(u => u.RoleId == supervisorRoleId).ToList();

                if (supervisorUsers.Count > 0)
                {
                    foreach (User u in supervisorUsers)
                    {

                        leaveRequestNotifications = _context.LeaveRequests.Where(lr => lr.UserId == u.Id && lr.ManagerNotified == false).ToList();

                        if (leaveRequestNotifications.Count > 0)
                        {
                            foreach (LeaveRequest lr in leaveRequestNotifications)
                            {

                                notificationMessage.Add($"Leave requested by {u.FirstName} {u.LastName} from {Convert.ToDateTime(lr.RequestedLeaveStartDate).ToShortDateString()} to {Convert.ToDateTime(lr.RequestedLeaveEndDate).ToShortDateString()}");

                            }
                        }
                    }
                }

            }


            ViewBag.NotificationMessage = notificationMessage;


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
