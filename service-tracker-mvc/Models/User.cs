using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace service_tracker_mvc.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [DisplayName("Open ID URL")]
        [MaxLength(2048)]
        public string ClaimedIdentifier { get; set; }

        [MaxLength(254)]
        public string Email { get; set; }

        [DisplayName("Role")]
        public int RoleId { get; set; }

        [DisplayName("First Login")]
        public DateTime? FirstLogin { get; set; }

        [DisplayName("Last Login")]
        public DateTime? LastLogin { get; set; }

        [DisplayName("Logins")]
        public int LoginCount { get; set; }

        [DisplayName("Associated Employee")]
        public int? ServicerId { get; set; }
        public virtual Servicer Servicer { get; set; }

        [DisplayName("Associated Organization")]
        public int? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}