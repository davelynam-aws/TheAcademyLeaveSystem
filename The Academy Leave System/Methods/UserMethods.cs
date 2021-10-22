using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Academy_Leave_System.Models;

namespace The_Academy_Leave_System.Methods
{
    public static class UserMethods
    {
        public static void SetCurrentUserVariables(User user)
        {
            CurrentUser.Email = user.Email;
            CurrentUser.FirstName = user.FirstName;
            CurrentUser.LastName = user.LastName;
            CurrentUser.Id = CurrentUser.Id = user.Id;
            
        }

        public static void ClearCurrentUserVariables()
        {
            CurrentUser.Email = null;
            CurrentUser.FirstName = null;
            CurrentUser.LastName = null;
            CurrentUser.Id = 0;
        }
    }
}
