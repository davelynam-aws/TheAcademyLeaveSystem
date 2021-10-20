using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace The_Academy_Leave_System.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [Column(name:"Id")]
        public int Id { get; set; }

        [Column(name:"RoleName")]
        public string RoleName { get; set; }
    }
}
