using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace service_tracker_mvc.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
    }
}