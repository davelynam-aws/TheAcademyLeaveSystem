using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace The_Academy_Leave_System.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the TALSIdentity class
    public class TALSIdentity : IdentityUser
    {
        [PersonalData]
        [DisplayName("First Name")]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [DisplayName("Last Name")]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
    }
}
