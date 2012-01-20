using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace service_tracker_mvc.Models
{
    public class Servicer
    {
        public int ServicerId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}