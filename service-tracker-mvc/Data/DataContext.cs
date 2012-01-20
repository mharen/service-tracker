using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using service_tracker_mvc.Models;

namespace service_tracker_mvc.Data
{
    public class DataContext :DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Servicer> Servicers { get; set; }
    }
}