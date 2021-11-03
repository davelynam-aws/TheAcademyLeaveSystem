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
    public class UsersController : Controller
    {
        private readonly DBContext _context;

        public UsersController(DBContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()

        {
            // Create new userviewmodel
            UserViewModel userViewModel = new UserViewModel();

            // Get role options
            List<Role> roles = _context.Roles.ToList();

            // Create an empty select list to convert role list.
            List<SelectListItem> roleList = new List<SelectListItem>();

            // Add first entry to role select list that cannot be selected.
            roleList.Add(new SelectListItem { Selected = true, Disabled = true, Value = "-1", Text = "-- Select a Role --" });

            // Populate select list with roles.
            foreach (Role role in roles)
            {
                roleList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.RoleName
                });
            }

            // Assign role list to userviewmodel list.
            userViewModel.RoleOptions = roleList;

            // Get role options
            List<Team> teams = _context.Teams.ToList();

            // Create an empty select list to convert role list.
            List<SelectListItem> teamList = new List<SelectListItem>();

            // Add first entry to role select list that cannot be selected.
            teamList.Add(new SelectListItem { Selected = true, Disabled = true, Value = "-1", Text = "-- Select a Team --" });

            // Populate select list with roles.
            foreach (Team team in teams)
            {
                teamList.Add(new SelectListItem
                {
                    Value = team.Id.ToString(),
                    Text = team.TeamName
                });
            }

            // Assign team list to userviewmodel list.
            userViewModel.TeamOptions = teamList;




            return View(userViewModel);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(user);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,TeamId,RoleId,FirstName,LastName,IsActive,LeaveAllowanceThisYear,LeaveAllowanceNextYear,Email,PasswordHash,LastLoggedInDateTime")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamId,RoleId,FirstName,LastName,IsActive,LeaveAllowanceThisYear,LeaveAllowanceNextYear,Email,PasswordHash,LastLoggedInDateTime")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
