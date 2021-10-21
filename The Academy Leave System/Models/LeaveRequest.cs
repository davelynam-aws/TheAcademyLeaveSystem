using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace The_Academy_Leave_System.Models
{
    [Table("LeaveRequests")]
    public class LeaveRequest
    {
        [Key]
        [Column(name:"Id")]
        public int Id { get; set; }

        [Column(name:"UserId")]
        public int UserId { get; set; }

        [Column(name:"CreatedDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [Column(name:"RequestedLeaveStartDate")]
        public DateTime RequestedLeaveStartDate { get; set; }

        [Column(name:"RequestedLeaveEndDate")]
        public DateTime RequestedLeaveEndDate { get; set; }

        [Column(name:"TotalDaysRequested")]
        public decimal TotalDaysRequested { get; set; }

        [Column(name:"HalfDayIdentification")]
        public string HalfDayIdentification { get; set; }

        [Column(name:"ApprovedDateTime")]
        public DateTime ApprovedDateTime { get; set; }

        [Column(name:"RejectedDateTime")]
        public DateTime RejectedDateTime { get; set; }

        [Column(name:"ManagerNotified")]
        public bool ManagerNotified { get; set; }

        [Column(name:"UserNotified")]
        public bool UserNotified { get; set; }

        [Column(name:"IsCancelled")]
        public bool IsCancelled { get; set; }

    }
}
