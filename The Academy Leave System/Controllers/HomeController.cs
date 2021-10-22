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

namespace The_Academy_Leave_System.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Register db context for TALS database.
        private readonly DBContext _db;
        

        public HomeController(ILogger<HomeController> logger, DBContext dBContext)
        {
            _logger = logger;
            _db = dBContext;
        }

        public IActionResult Index()
        {
            // If the user is not logged in then always redirect to the login page.
            // This should be present on each page to prevent unauthorised access and errors.
            if (CurrentUser.Email == null)
            {
                return LocalRedirect("/Identity/Account/Login");   
            }



            int userCount = _db.Users.Count();



            List<User> userList = new List<User>();
            userList = _db.Users.ToList();

            // Hash
            var hash = SecurePasswordHasher.Hash("Password1");

            // Verify
            var result = SecurePasswordHasher.Verify("Password1", hash);


            return View();
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
