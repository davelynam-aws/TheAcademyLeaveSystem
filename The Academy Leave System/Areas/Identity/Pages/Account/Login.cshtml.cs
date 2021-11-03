using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using The_Academy_Leave_System.Areas.Identity.Data;
using The_Academy_Leave_System.Methods;
using The_Academy_Leave_System.Models;
using Microsoft.EntityFrameworkCore;

namespace The_Academy_Leave_System.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<TALSIdentity> _userManager;
        private readonly SignInManager<TALSIdentity> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        // Register db context for TALS database.
        private readonly DBContext _db;
        private User _currentUser;

        public LoginModel(SignInManager<TALSIdentity> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<TALSIdentity> userManager,
            DBContext dBContext,
            User currentUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _db = dBContext;
            _currentUser = currentUser;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {

                //var hash = SecurePasswordHasher.Hash(Input.Password);

                var retrievedHash = _db.Users.Where(u => u.Email == Input.Email).Select(u => u.PasswordHash).Single();

                if (retrievedHash != null)
                {
                    var verifyResult = SecurePasswordHasher.Verify(Input.Password, retrievedHash);

                    if (verifyResult == true)
                    {
                        // Authentication successful.

                       
                        _currentUser = _db.Users.Where(u => u.Email == Input.Email).Single();

                        _currentUser.LastLoggedInDateTime = DateTime.Now;

                        //Update the db and save.
                        _db.Entry(_currentUser).State = EntityState.Modified;
                        _db.SaveChanges();

                        UserMethods.SetCurrentUserVariables(_currentUser, _db);

               

                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }


                    // Authentication successful.
                    //await _signInManager.SignInAsync(User, isPersistent: true);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }



                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //}
                //if (result.IsLockedOut)
                //{
                //    _logger.LogWarning("User account locked out.");
                //    return RedirectToPage("./Lockout");
                //}
                //else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
