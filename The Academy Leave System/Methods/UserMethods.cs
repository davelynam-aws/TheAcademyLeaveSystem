using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Academy_Leave_System.Models;

namespace The_Academy_Leave_System.Methods
{
    public static class UserMethods



    {
        public static void SetCurrentUserVariables(User user, DBContext _db)
        {
            CurrentUser.Email = user.Email;
            CurrentUser.FirstName = user.FirstName;
            CurrentUser.LastName = user.LastName;
            CurrentUser.Id = user.Id;
            CurrentUser.RoleId = user.RoleId;

            var roles = _db.Roles.ToList();

            // Get Role name from role Id.
            CurrentUser.Role = _db.Roles.Where(r => r.Id == CurrentUser.RoleId).Select(r => r.RoleName).Single();
        }

        public static void ClearCurrentUserVariables()
        {
            CurrentUser.Email = null;
            CurrentUser.FirstName = null;
            CurrentUser.LastName = null;
            CurrentUser.Id = 0;
            CurrentUser.RoleId = 0;
            CurrentUser.Role = "";
        }


    }
}
