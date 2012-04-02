using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace service_tracker_mvc.Models
{
    public class Invitation
    {
        [DisplayName("Associated User")]
        [Key]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        [DisplayName("Code")]
        public Guid InvitationId { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Sent Date")]
        public DateTime? SentDate { get; set; }

        [DisplayName("Accepted Date")]
        public DateTime? AcceptedDate { get; set; }
    }
}