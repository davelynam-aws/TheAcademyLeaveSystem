using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace The_Academy_Leave_System.Models
{
    public class DBContext : DbContext
    {
        // Constructor for the DBContext.
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
    }
}
