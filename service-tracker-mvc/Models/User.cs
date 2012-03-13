using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(2048), Required]
        public string ClaimedIdentifier { get; set; }

        [MaxLength(254)]
        public string Email { get; set; }

        public int RoleId { get; set; }
        
        public DateTime FirstLogin { get; set; }
        
        public DateTime LastLogin { get; set; }
        
        public int LoginCount { get; set; }
    }
}