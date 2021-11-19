using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using The_Academy_Leave_System.Methods;
using The_Academy_Leave_System.Models;
using The_Academy_Leave_System.ViewModels;

namespace The_Academy_Leave_System.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private readonly DBContext _context;

        public LeaveRequestsController(DBContext context)
        {
            _context = context;
        }

        // GET: LeaveRequests for a user
        public async Task<IActionResult> Index()
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }



            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // create list of leave request view models.
            List<LeaveRequestViewModel> leaveRequestViewModels = new List<LeaveRequestViewModel>();

            // Get leave requests for this user.
            var leaveRequests = await _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id).OrderByDescending(l => l.CreatedDateTime).ToListAsync();

            // Generate model data.
            if (leaveRequests != null)
            {
                foreach(LeaveRequest lr in leaveRequests)
                {
                    LeaveRequestViewModel lrvm = new LeaveRequestViewModel();

                    // Establish automated status depending on approval status.
                    if(lr.IsCancelled == true)
                    {
                        lrvm.Status = "Cancelled";

                    }
                    else
                    {
                        if (lr.ApprovedDateTime == DateTime.Parse("01/01/1753") && lr.RejectedDateTime == DateTime.Parse("01/01/1753"))
                        {
                            lrvm.Status = "Pending";
                        }
                        if (lr.ApprovedDateTime != DateTime.Parse("01/01/1753"))
                        {
                            lrvm.Status = "Approved";
                        }
                        if (lr.RejectedDateTime != DateTime.Parse("01/01/1753"))
                        {
                            lrvm.Status = "Denied";
                        }
                    }



                    // Add to list.
                    lrvm.ThisLeaveRequest = lr;
                    leaveRequestViewModels.Add(lrvm);
                }
            }


            // Send clashes to the view if they exist.
            if (GlobalClashes.Clashes.Count > 0)
            {
                ViewBag.Clashes = GlobalClashes.Clashes;
            }


            return View(leaveRequestViewModels);
        }

        // GET: Get Leave Requests for the supervisor's users.
        public async Task<IActionResult> SupervisorIndex()
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // Validate user access.
            if (CurrentUser.Role != "Supervisor")
            {
                return RedirectToAction("Index", "Home");
            }

            // Create new list of request view models.
            List<LeaveRequestViewModel> leaveRequestViewModels = new List<LeaveRequestViewModel>();
            LeaveRequestViewModel leaveRequestViewModel;

            // Default null date.
            DateTime defaultDateTime = DateTime.Parse("01/01/1753");

            // Get the team ID for this supervisor.
            int myTeamId = await _context.Users.Where(u => u.Id == CurrentUser.Id).Select(u => u.TeamId).SingleOrDefaultAsync();

            // Get all users with the supervisors team id, excluding the current supervisor.
            var myUsers = await _context.Users.Where(u => u.TeamId == myTeamId && u.Id != CurrentUser.Id).ToListAsync();

            // If the supervisor is responsible for users, loop through them.
            if(myUsers.Count > 0)
            {
                foreach(User u in myUsers)
                {
                    // Get all pending leave requests for each user.
                    var userLeaveRequests = await _context.LeaveRequests.Where(lr => lr.UserId == u.Id && lr.ApprovedDateTime == defaultDateTime && lr.RejectedDateTime == defaultDateTime && lr.IsCancelled == false).ToListAsync();

                    // If this user has requests then create a view model entry for them to be displayed.
                    if (userLeaveRequests.Count > 0)
                    {
                        foreach(LeaveRequest lr in userLeaveRequests)
                        {
                            leaveRequestViewModel = new LeaveRequestViewModel();
                            leaveRequestViewModel.FullName = $"{u.FirstName} {u.LastName}";
                            leaveRequestViewModel.ThisLeaveRequest = lr;
                            leaveRequestViewModels.Add(leaveRequestViewModel);
                        }
                    }              
                }
            }


            return View(leaveRequestViewModels);
        }

        // Approve a leave request id
        public async Task<IActionResult> SupervisorApproveLeave(int? id)
        {

            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // Validate user access.
            if (CurrentUser.Role != "Supervisor")
            {
                return RedirectToAction("Index", "Home");
            }

            // Get and populate the leave request that is being approved.
            var leaveRequest = await _context.LeaveRequests.Where(lr => lr.Id == id).SingleOrDefaultAsync();

            if (leaveRequest != null)
            {
                leaveRequest.ApprovedDateTime = DateTime.Now;
                leaveRequest.ManagerNotified = true;

                // Update database.
                try
                {
                    _context.Update(leaveRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestExists(leaveRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SupervisorIndex));
            }

            return View();
        }


        // Deny a leave request id
        public async Task<IActionResult> SupervisorDenyLeave(int? id)
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // Validate user access.
            if (CurrentUser.Role != "Supervisor")
            {
                return RedirectToAction("Index", "Home");
            }


            // Get this leave request.
            var leaveRequest = await _context.LeaveRequests.Where(lr => lr.Id == id).SingleOrDefaultAsync();

            if (leaveRequest != null)
            {
                leaveRequest.RejectedDateTime = DateTime.Now;
                leaveRequest.ManagerNotified = true;

                // Update database.
                try
                {
                    _context.Update(leaveRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestExists(leaveRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SupervisorIndex));
            }

            return View();
        }


        // GET: Get Leave Requests for the Manager's users.
        public async Task<IActionResult> ManagerIndex()
        {

            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // Validate user access.
            if (CurrentUser.Role != "Manager")
            {
                return RedirectToAction("Index", "Home");
            }

            // Create new list of request view models.
            List<LeaveRequestViewModel> leaveRequestViewModels = new List<LeaveRequestViewModel>();
            LeaveRequestViewModel leaveRequestViewModel;

            // Default null date.
            DateTime defaultDateTime = DateTime.Parse("01/01/1753");

            int supervisorRoleId = _context.Roles.Where(r => r.RoleName == "Supervisor").Select(r => r.Id).Single();

            // Get the team ID for this Manager.
            //int myTeamId = await _context.Users.Where(u => u.Id == CurrentUser.Id).Select(u => u.TeamId).SingleOrDefaultAsync();

            // Get all supervisors with the manager's team id, excluding the current manager.
            var myUsers = await _context.Users.Where(u => /*u.TeamId == myTeamId &&*/ u.Id != CurrentUser.Id && u.RoleId == supervisorRoleId).ToListAsync();

            // If the manager is responsible for supervisors, loop through them.
            if (myUsers.Count > 0)
            {
                foreach (User u in myUsers)
                {
                    // Get all pending leave requests for each user.
                    var userLeaveRequests = await _context.LeaveRequests.Where(lr => lr.UserId == u.Id && lr.ApprovedDateTime == defaultDateTime && lr.RejectedDateTime == defaultDateTime && lr.IsCancelled == false).ToListAsync();

                    // If this user has requests then create a view model entry for them to be displayed.
                    if (userLeaveRequests.Count > 0)
                    {
                        foreach (LeaveRequest lr in userLeaveRequests)
                        {
                            leaveRequestViewModel = new LeaveRequestViewModel();
                            leaveRequestViewModel.FullName = $"{u.FirstName} {u.LastName}";
                            leaveRequestViewModel.ThisLeaveRequest = lr;
                            leaveRequestViewModels.Add(leaveRequestViewModel);
                        }
                    }
                }
            }


            return View(leaveRequestViewModels);
        }


        // Approve a leave request id
        public async Task<IActionResult> ManagerApproveLeave(int? id)
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // Validate user access.
            if (CurrentUser.Role != "Manager")
            {
                return RedirectToAction("Index", "Home");
            }

            // Get leave request.
            var leaveRequest = await _context.LeaveRequests.Where(lr => lr.Id == id).SingleOrDefaultAsync();

            if (leaveRequest != null)
            {
                leaveRequest.ApprovedDateTime = DateTime.Now;
                leaveRequest.ManagerNotified = true;

                // Update database.
                try
                {
                    _context.Update(leaveRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestExists(leaveRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SupervisorIndex));
            }

            return View();
        }


        // Deny a leave request id
        public async Task<IActionResult> ManagerDenyLeave(int? id)
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // Validate user access.
            if (CurrentUser.Role != "Manager")
            {
                return RedirectToAction("Index", "Home");
            }

            var leaveRequest = await _context.LeaveRequests.Where(lr => lr.Id == id).SingleOrDefaultAsync();

            if (leaveRequest != null)
            {
                // Get leave request
                leaveRequest.RejectedDateTime = DateTime.Now;
                leaveRequest.ManagerNotified = true;
                
                // Update database.
                try
                {
                    _context.Update(leaveRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestExists(leaveRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SupervisorIndex));
            }

            return View();
        }

        public async Task<IActionResult> EmployeeDataIndex()
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // Validate user access.
            if (CurrentUser.Role != "Manager")
            {
                return RedirectToAction("Index", "Home");
            }

            LeaveRequestViewModel leaveRequestViewModel = new LeaveRequestViewModel();

            // Create an empty select list to convert role list.
            List<User> userList = await _context.Users.ToListAsync();

            // Add first entry to role select list that cannot be selected.
            // roleList.Add(new SelectListItem { Selected = true, Disabled = true, Value = "-1", Text = "-- Select a Role --" });

            // Populate select list with roles.
            foreach (User user in userList)
            {
                leaveRequestViewModel.Users.Add(new SelectListItem
                {
                    Value = user.Id.ToString(),
                    Text = $"{user.FirstName} {user.LastName}"
                });
            }


            return View(leaveRequestViewModel);
        }


        public async Task<IActionResult> EmployeeDataReport(LeaveRequestViewModel leaveRequestViewModel)
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // Validate user access.
            if (CurrentUser.Role != "Manager")
            {
                return RedirectToAction("Index", "Home");
            }

            if (leaveRequestViewModel.SelectedUserId != 0)
            {
                // If user has been selected, show reporing data.
                leaveRequestViewModel.ThisUser = _context.Users.Where(u => u.Id == leaveRequestViewModel.SelectedUserId).Single();
                leaveRequestViewModel.ThisUserLeaveRequests = await _context.LeaveRequests.Where(lr => lr.UserId == leaveRequestViewModel.SelectedUserId).ToListAsync();




                return View("EmployeeData", leaveRequestViewModel);

            }




            return RedirectToAction("Index", "Home");
        }




        // GET: LeaveRequests/Create
        public IActionResult Create()
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            LeaveRequestViewModel leaveRequestViewModel = new LeaveRequestViewModel();
            leaveRequestViewModel.ThisLeaveRequest = new LeaveRequest();
            leaveRequestViewModel.ThisLeaveRequest.RequestedLeaveStartDate = null;
            leaveRequestViewModel.ThisLeaveRequest.RequestedLeaveEndDate = null;
            

            return View(leaveRequestViewModel);
        }

        // POST: LeaveRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestViewModel leaveRequestViewModel)
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            if (ModelState.IsValid)
            {
                ////populate static values here
                leaveRequestViewModel.ThisLeaveRequest.UserId = CurrentUser.Id;
                leaveRequestViewModel.ThisLeaveRequest.ApprovedDateTime = DateTime.Parse("01/01/1753");
                leaveRequestViewModel.ThisLeaveRequest.CreatedDateTime = DateTime.Now;
                leaveRequestViewModel.ThisLeaveRequest.IsCancelled = false;
                leaveRequestViewModel.ThisLeaveRequest.ManagerNotified = false;
                leaveRequestViewModel.ThisLeaveRequest.RejectedDateTime = DateTime.Parse("01/01/1753");
                leaveRequestViewModel.ThisLeaveRequest.UserNotified = false;

                
                if (leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification == null)
                {
                    leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification = "NA";
                }
                else
                {
                    if (leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification.Contains("/"))
                    {
                        leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification = leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification.Replace("/", "");
                    }
                }

                // Get any leave clashes.
                List<LeaveRequestViewModel> clashes = CheckForLeaveClashes(leaveRequestViewModel);


                // Set as global variable so it can be accessed by the view.
                GlobalClashes.Clashes = clashes;

                _context.Add(leaveRequestViewModel.ThisLeaveRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveRequestViewModel);
        }

        // GET: LeaveRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            LeaveRequestViewModel leaveRequestViewModel = new LeaveRequestViewModel();
            leaveRequestViewModel.ThisLeaveRequest = leaveRequest;

            if (leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification == "NA")
            {
                leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification = "N/A";
            }

            return View(leaveRequestViewModel);
        }

        // POST: LeaveRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveRequestViewModel leaveRequestViewModel)
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            if (id != leaveRequestViewModel.ThisLeaveRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification == null)
                {
                    leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification = "NA";
                }
                else
                {
                    if (leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification.Contains("/"))
                    {
                        leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification = leaveRequestViewModel.ThisLeaveRequest.HalfDayIdentification.Replace("/", "");
                    }
                }

                try
                {
                    _context.Update(leaveRequestViewModel.ThisLeaveRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestExists(leaveRequestViewModel.ThisLeaveRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveRequestViewModel);
        }

        // GET: LeaveRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }

            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // POST: LeaveRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Test online and database connectivity.
            if (OnlineConnectivity.CheckDatabaseAvailability(_context) == false)
            {
                return View("ConnectivityIssue");
            }


            // Redirect user to login screen if they are not logged in.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            leaveRequest.IsCancelled = true;

            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveRequestExists(int id)
        {
            return _context.LeaveRequests.Any(e => e.Id == id);
        }



        public List<LeaveRequestViewModel> CheckForLeaveClashes(LeaveRequestViewModel leaveRequestViewModel)
        {

            List<LeaveRequestViewModel> leaveClashesViewModels = new List<LeaveRequestViewModel>();
            LeaveRequestViewModel leaveRequestViewModelLocal;
           List<int> addedRequests = new List<int>();

            DateTime nullDateDefault = DateTime.Parse("01/01/1753");

            // Gets clashes where start date is between clash dates.
            List<LeaveRequest> leaveRequestClashes =  _context.LeaveRequests.Where(lr => leaveRequestViewModel.ThisLeaveRequest.RequestedLeaveStartDate >= lr.RequestedLeaveStartDate && leaveRequestViewModel.ThisLeaveRequest.RequestedLeaveStartDate <= lr.RequestedLeaveEndDate && lr.IsCancelled != true && lr.RejectedDateTime == nullDateDefault).ToList();


            // Get clashes where end date is between clash dates.
            List<LeaveRequest> leaveRequestClashesAdditional =  _context.LeaveRequests.Where(lr => leaveRequestViewModel.ThisLeaveRequest.RequestedLeaveEndDate >= lr.RequestedLeaveStartDate && leaveRequestViewModel.ThisLeaveRequest.RequestedLeaveEndDate <= lr.RequestedLeaveEndDate && lr.IsCancelled != true && lr.RejectedDateTime == nullDateDefault).ToList();


            if(leaveRequestClashes.Count > 0)
            {
   
               foreach(LeaveRequest lrc in leaveRequestClashes)
                {
                    // Add the clash to the master list if has not already been added.
                    if (!addedRequests.Contains(lrc.Id))
                    {
                        leaveRequestViewModelLocal = new LeaveRequestViewModel();
                        leaveRequestViewModelLocal.ThisLeaveRequest = lrc;
                        leaveRequestViewModelLocal.ThisUser = _context.Users.Where(u => u.Id == lrc.UserId).Single();
                        leaveRequestViewModelLocal.FullName = $"{leaveRequestViewModelLocal.ThisUser.FirstName} {leaveRequestViewModelLocal.ThisUser.LastName}";
                        leaveClashesViewModels.Add(leaveRequestViewModelLocal);
                        addedRequests.Add(lrc.Id);
                    }

                }
            }

            if (leaveRequestClashesAdditional.Count > 0)
            {
                foreach (LeaveRequest lrc in leaveRequestClashesAdditional)
                {
                    // Add the clash to the master list if has not already been added.
                    if (!addedRequests.Contains(lrc.Id))
                    {
                        leaveRequestViewModelLocal = new LeaveRequestViewModel();
                        leaveRequestViewModelLocal.ThisLeaveRequest = lrc;
                        leaveRequestViewModelLocal.ThisUser = _context.Users.Where(u => u.Id == lrc.UserId).Single();
                        leaveRequestViewModelLocal.FullName = $"{leaveRequestViewModelLocal.ThisUser.FirstName} {leaveRequestViewModelLocal.ThisUser.LastName}";
                        leaveClashesViewModels.Add(leaveRequestViewModelLocal);
                        addedRequests.Add(lrc.Id);
                    }

                }
            }


            // Return a list of clashes to display to the user.



            return leaveClashesViewModels;
        }







    }
}
