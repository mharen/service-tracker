using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace service_tracker_mvc.Models
{
    //Customers - one "row" per site with the following info:
    // - Number, e.g. 2301
    // - Name, e.g. Home Depot
    // - Address, e.g. 2600 Hurstborne Pkwy...
    // - Vendor Number, e.g. 42957
    // - (eventually expand to include anything useful, e.g. region, manager, etc.)
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string VendorNumber { get; set; }
    }
}