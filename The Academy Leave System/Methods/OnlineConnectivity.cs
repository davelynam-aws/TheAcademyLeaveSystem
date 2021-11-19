using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using The_Academy_Leave_System.Models;

namespace The_Academy_Leave_System.Methods
{
    public static class OnlineConnectivity
    {
        /// <summary>
        /// This method checks for connectivity to the database and returns a boolean as the result.
        /// </summary>
        /// <returns></returns>
        public static bool CheckDatabaseAvailability(DBContext _context)
        {
            bool connectionValid = false;

            // Test the connection to the database.
            try
            {
                if (_context.Roles.Count() > 0)
                {
            
                }
                connectionValid = true;
            }
            catch (Exception)
            {

                connectionValid = false;
            }

            // Test the connection to google to simulate online connectivity for debugging. Local connection can be disabled for testing.
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    connectionValid = true;
            }
            catch
            {
                connectionValid = false;
            }

            return connectionValid;
        }


    }
}
