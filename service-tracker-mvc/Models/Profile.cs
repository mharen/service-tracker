using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace service_tracker_mvc.Models
{
    public class Profile
    {
        public int Id { get; set; }
        
        [MaxLength(50), Required]
        public string Name { get; set; }
        
        [MaxLength(2048)]
        public string AboutUrl { get; set; }

        [MaxLength(100)]
        public string EmailFromAddress { get; set; }

        [MaxLength(100)]
        public string EmailBccAddress { get; set; }
    }
}