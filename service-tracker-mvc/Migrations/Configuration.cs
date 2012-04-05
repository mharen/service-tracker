namespace service_tracker_mvc.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using service_tracker_mvc.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<service_tracker_mvc.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Data.DataContext context)
        {
            base.Seed(context);

            if (!context.Profiles.Any())
            {
                context.Profiles.Add(new Profile() { Name = "Super Acme Store, LLC", AboutUrl = "http://blog.wassupy.com" });
            }

            if (!context.Sites.Any())
            {
                context.Sites.Add(new Site() { Name = "Store A", Address = "123 Main Street" });
                context.Sites.Add(new Site() { Name = "Store B", Address = "456 High Street" });
                context.Sites.Add(new Site() { Name = "Store C", Address = "789 North Street" });
            }

            if (!context.Services.Any())
            {
                context.Services.Add(new Service() { Sku = "123456789", Description = "Service A", Cost = 100m });
                context.Services.Add(new Service() { Sku = "456789123", Description = "Service B", Cost = 200m });
                context.Services.Add(new Service() { Sku = "789123456", Description = "Service C", Cost = 300m });
            }

            if (!context.Servicers.Any())
            {
                context.Servicers.Add(new Servicer() { Name = "Ashley" });
                context.Servicers.Add(new Servicer() { Name = "Bill" });
                context.Servicers.Add(new Servicer() { Name = "Chris" });
            }

            context.SaveChanges();

            if (!context.Invoices.Any())
            {
                context.Invoices.Add(new Invoice()
                {
                    SiteId = 1,
                    FrtBill = "frtbill 1  3",
                    KeyRec = "Key Rec  b c",
                    PurchaseOrder = "PO 123 abc",
                    ServiceDate = DateTime.UtcNow.Date,
                    ServicerId = 1
                });
                context.Invoices.Add(new Invoice()
                {
                    SiteId = 1,
                    FrtBill = "frtbill 1 2  4",
                    KeyRec = "Key a b d",
                    PurchaseOrder = " 123 abcd",
                    ServiceDate = DateTime.UtcNow.Date.AddDays(-1),
                    ServicerId = 1
                });
                context.Invoices.Add(new Invoice()
                {
                    SiteId = 1,
                    FrtBill = "frtbill  2 3",
                    KeyRec = "Key Rec a ",
                    PurchaseOrder = "PO 123 ",
                    ServiceDate = DateTime.UtcNow.Date.AddDays(-1),
                    ServicerId = 1
                });
                context.Invoices.Add(new Invoice()
                {
                    SiteId = 1,
                    KeyRec = " Rec  c",
                    PurchaseOrder = "PO  abc",
                    ServiceDate = DateTime.UtcNow.Date.AddDays(-3),
                    ServicerId = 1
                });

                context.SaveChanges();

                context.InvoiceItems.Add(new InvoiceItem()
                {
                    InvoiceId = 1,
                    InvoiceItemId = 1,
                    ServiceId = 1,
                    Quantity = 2m
                });
                context.InvoiceItems.Add(new InvoiceItem()
                {
                    InvoiceId = 1,
                    InvoiceItemId = 2,
                    ServiceId = 1,
                    Quantity = 2m,
                });
                context.InvoiceItems.Add(new InvoiceItem()
                {
                    InvoiceId = 1,
                    InvoiceItemId = 3,
                    ServiceId = 1,
                    Quantity = 3m,
                });
                context.InvoiceItems.Add(new InvoiceItem()
                {
                    InvoiceId = 1,
                    InvoiceItemId = 4,
                    ServiceId = 2,
                    Quantity = 4m,
                });
                context.InvoiceItems.Add(new InvoiceItem()
                {
                    InvoiceId = 1,
                    InvoiceItemId = 5,
                    ServiceId = 2,
                    Quantity = 5m
                });

                context.SaveChanges();
            }
        }
    }
}
