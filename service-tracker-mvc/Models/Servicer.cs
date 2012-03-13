using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Models
{
    public class Servicer
    {
        public int ServicerId { get; set; }
        
        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}