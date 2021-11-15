using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //if(id != CurrentUser.Id)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            if(CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            List<LeaveRequestViewModel> leaveRequestViewModels = new List<LeaveRequestViewModel>();

            var leaveRequests = await _context.LeaveRequests.Where(l => l.UserId == CurrentUser.Id).OrderByDescending(l => l.CreatedDateTime).ToListAsync();

            if (leaveRequests != null)
            {
                foreach(LeaveRequest lr in leaveRequests)
                {
                    LeaveRequestViewModel lrvm = new LeaveRequestViewModel();

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




                    lrvm.ThisLeaveRequest = lr;
                    leaveRequestViewModels.Add(lrvm);
                }
            }


            return View(leaveRequestViewModels);
        }

        // GET: Get Leave Requests for the supervisor's users.
        public async Task<IActionResult> SupervisorIndex()
        {
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



        // GET: LeaveRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: LeaveRequests/Create
        public IActionResult Create()
        {
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

                _context.Add(leaveRequestViewModel.ThisLeaveRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveRequestViewModel);
        }

        // GET: LeaveRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
    }
}
