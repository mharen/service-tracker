using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace service_tracker_mvc.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
    }
}