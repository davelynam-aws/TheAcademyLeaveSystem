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

        [Display(Name = "Request Date")]
        [Column(name:"CreatedDateTime")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [Column(name:"RequestedLeaveStartDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? RequestedLeaveStartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [Column(name:"RequestedLeaveEndDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? RequestedLeaveEndDate { get; set; }

        [Display(Name = "Days Requested")]
        [Column(name:"TotalDaysRequested")]
        public decimal TotalDaysRequested { get; set; }

        [Display(Name = "AM/PM")]
        [Column(name:"HalfDayIdentification")]
        public string HalfDayIdentification { get; set; }

        [Display(Name = "Approved Date")]
        [Column(name:"ApprovedDateTime")]
        public DateTime ApprovedDateTime { get; set; }

        [Display(Name = "Rejected Date")]
        [Column(name:"RejectedDateTime")]
        public DateTime RejectedDateTime { get; set; }

        [Column(name:"ManagerNotified")]
        public bool ManagerNotified { get; set; }

        [Column(name:"UserNotified")]
        public bool UserNotified { get; set; }

        [Display(Name = "Cancelled")]
        [Column(name:"IsCancelled")]
        public bool IsCancelled { get; set; }

    }
}
