﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace service_tracker_mvc.Models
{
    //Customers - one "row" per site with the following info:
    // - Number, e.g. 2301
    // - Name, e.g. Home Depot
    // - Address, e.g. 2600 Hurstborne Pkwy...
    // - Vendor Number, e.g. 42957
    // - (eventually expand to include anything useful, e.g. region, manager, etc.)
    public class Site
    {
        public int SiteId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        
        [DataType(DataType.MultilineText)]
        [MaxLength(250)]
        public string Address { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        
        public virtual List<Invoice> Invoices { get; set; }

        public override string ToString()
        {
            return Name; 
        }
    }
}