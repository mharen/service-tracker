using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace service_tracker_mvc.Models
{
    public class InvitationLog
    {
        [Key]
        public int InvitationLogId { get; set; }

        [DisplayName("User")]
        public int UserId { get; set; }

        [DisplayName("Action")]
        public int Action { get; set; }

        [DisplayName("Log Date (UTC)")]
        public DateTime LogDate { get; set; }
    }
}