using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace The_Academy_Leave_System.Models
{
    [Table("EventLog")]
    public class EventLog
    {
        [Key]
        [Column(name:"Id")]
        public int Id { get; set; }

        [Column(name:"CreatedByUserId")]
        public int CreatedByUserId { get; set; }

        [Column(name:"LeaveRequestId")]
        public int LeaveRequestId { get; set; }

        [Column(name:"EventDetails")]
        public string EventDetails { get; set; }
    }
}
