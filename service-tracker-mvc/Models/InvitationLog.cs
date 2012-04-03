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
        [DisplayName("User")]
        [Key]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [DisplayName("Action")]
        public InvitationAction Action { get; set; }
    }
}