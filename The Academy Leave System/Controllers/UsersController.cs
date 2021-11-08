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
            // Redirect to home if user is not authorised.
            if(CurrentUser.Role != "Administrator")
            {
                return RedirectToAction("Index", "Home");
            }

            // Return all users ordered by first name.
            return View(await _context.Users.OrderBy(u => u.FirstName).ToListAsync());
        }

      

        // GET: Users/Create
        public IActionResult Create()

        {
            // Redirect to home if user is not authorised.
            if (CurrentUser.Role != "Administrator")
            {
                return RedirectToAction("Index", "Home");
            }

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
            // Redirect to home if user is not authorised.
            if (CurrentUser.Role != "Administrator")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // Generate Password has from input value.
                userViewModel.ThisUser.PasswordHash = SecurePasswordHasher.Hash(userViewModel.ThisUser.Password);
                // Set user to active.
                userViewModel.ThisUser.IsActive = true;
                // Set default date.
                userViewModel.ThisUser.LastLoggedInDateTime = DateTime.Parse("01/01/1753");

                // Insert new user record.
                _context.Add(userViewModel.ThisUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


                //_context.Add(user);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }

            // Return with validation message if model is invalid.
            return View(userViewModel);
        }

       
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Redirect to home if user is not authorised.
            if (CurrentUser.Role != "Administrator")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            UserViewModel userViewModel = new UserViewModel();
            userViewModel.ThisUser = user;

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

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel userViewModel)
        {
            // Redirect to home if user is not authorised.
            if (CurrentUser.Role != "Administrator")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != userViewModel.ThisUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Set user's current password hash so it is not overwritten with a null value.
                userViewModel.ThisUser.PasswordHash = _context.Users.Where(u => u.Id == id).Select(u => u.PasswordHash).Single();
                userViewModel.ThisUser.LastLoggedInDateTime = _context.Users.Where(u => u.Id == id).Select(u => u.LastLoggedInDateTime).Single();

                try
                {
                    _context.Update(userViewModel.ThisUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userViewModel.ThisUser.Id))
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
            return View(userViewModel);
        }
     
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // GET: Users/ResetPassword/5
        public IActionResult ResetPassword(int? id)
        {
            // Redirect to home if user is not authorised.
            if (CurrentUser.Role != "Administrator")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            ResetPassword resetPassword = new ResetPassword();
     
            return View(resetPassword);
        }

        // POST: Users/ResetPassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(int id, ResetPassword resetPassword)
        {
            // Redirect to home if user is not authorised.
            if (CurrentUser.Role != "Administrator")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                User user = _context.Users.Where(u => u.Id == id).Single();
                user.PasswordHash = SecurePasswordHasher.Hash(resetPassword.Password);

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
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
            return View(resetPassword);
        }



    }
}
