using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace The_Academy_Leave_System.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column(name:"Id")]
        public int Id { get; set; }

        [Column(name: "TeamId")]
        public int TeamId { get; set; }

        [Column(name: "RoleId")]
        public int RoleId { get; set; }

        [Required]
        [Column(name: "FirstName")]
        [DisplayName("Fist Name")]
        public string FirstName { get; set; }

        [Required]
        [Column(name: "LastName")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        
        [Column(name: "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [Column(name: "LeaveAllowanceThisYear")]
        public decimal LeaveAllowanceThisYear { get; set; }

        [Required]
        [Column(name: "LeaveAllowanceNextYear")]
        public decimal LeaveAllowanceNextYear { get; set; }

        [Required]
        [Column(name: "Email")]
        [DisplayName("Email")]
        public string Email { get; set; }


        [Column(name: "PasswordHash")]
        public string PasswordHash { get; set; }

        [Column(name: "LastLoggedInDateTime")]
        [DisplayName("Last Logged In")]
        public DateTime LastLoggedInDateTime { get; set; }

        [NotMapped]
        public string Password { get; set; }

    }
}
