using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace The_Academy_Leave_System.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [Column(name:"Id")]
        public int Id { get; set; }

        [Column(name: "TeamId")]
        public int TeamId { get; set; }

        [Column(name: "RoleId")]
        public int RoleId { get; set; }

        [Column(name: "FirstName")]
        public string FirstName { get; set; }

        [Column(name: "LastName")]
        public string LastName { get; set; }

        [Column(name: "IsActive")]
        public bool IsActive { get; set; }

        [Column(name: "LeaveAllowanceThisYear")]
        public decimal LeaveAllowanceThisYear { get; set; }

        [Column(name: "LeaveAllowanceNextYear")]
        public decimal LeaveAllowanceNextYear { get; set; }

        [Column(name: "Email")]
        public string Email { get; set; }

        [Column(name: "PasswordHash")]
        public string PasswordHash { get; set; }

        [Column(name: "LastLoggedInDateTime")]
        public DateTime LastLoggedInDateTime { get; set; }
    }
}
