using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using service_tracker_mvc.Models;
using System.Data.Entity.Infrastructure;

namespace service_tracker_mvc.Data
{
    public class DataContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>(); 
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Site> Sites { get; set; }
        public DbSet<Servicer> Servicers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvitationLog> InvitationLogs { get; set; }
    }
}