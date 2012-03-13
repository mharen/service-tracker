using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using service_tracker_mvc.Models;

namespace service_tracker_mvc.Data
{
    public static class Utilites
    {
        public static void SeedData()
        {
            using (var DB = new DataContext())
            {
                if (!DB.Profiles.Any())
                {
                    DB.Profiles.Add(new Profile() { Name = "Super Acme Store, LLC", AboutUrl = "http://blog.wassupy.com" });
                }

                if (!DB.Sites.Any())
                {
                    DB.Sites.Add(new Site() { Name = "Store A", Address = "123 Main Street" });
                    DB.Sites.Add(new Site() { Name = "Store B", Address = "456 High Street" });
                    DB.Sites.Add(new Site() { Name = "Store C", Address = "789 North Street" });
                }

                if (!DB.Products.Any())
                {
                    DB.Products.Add(new Product() { Manufacturer = "Mfg A", Description = "Product A" });
                    DB.Products.Add(new Product() { Manufacturer = "Mfg A", Description = "Product B" });
                    DB.Products.Add(new Product() { Manufacturer = "Mfg B", Description = "Product C" });
                }

                if (!DB.Services.Any())
                {
                    DB.Services.Add(new Service() { Sku = "123456789", Description = "Service A", Cost = 100m });
                    DB.Services.Add(new Service() { Sku = "456789123", Description = "Service B", Cost = 200m });
                    DB.Services.Add(new Service() { Sku = "789123456", Description = "Service C", Cost = 300m });
                }

                if (!DB.Servicers.Any())
                {
                    DB.Servicers.Add(new Servicer() { Name = "Ashley" });
                    DB.Servicers.Add(new Servicer() { Name = "Bill" });
                    DB.Servicers.Add(new Servicer() { Name = "Chris" });
                }

                DB.SaveChanges();

                if (!DB.Invoices.Any())
                {
                    DB.Invoices.Add(new Invoice()
                    {
                        SiteId = 1,
                        FrtBill = "frtbill 1  3",
                        KeyRec = "Key Rec  b c",
                        PurchaseOrder = "PO 123 abc",
                        ServiceDate = DateTime.UtcNow.Date,
                        ServicerId = 1
                    });
                    DB.Invoices.Add(new Invoice()
                    {
                        SiteId = 1,
                        FrtBill = "frtbill 1 2  4",
                        KeyRec = "Key a b d",
                        PurchaseOrder = " 123 abcd",
                        ServiceDate = DateTime.UtcNow.Date.AddDays(-1),
                        ServicerId = 1
                    });
                    DB.Invoices.Add(new Invoice()
                    {
                        SiteId = 1,
                        FrtBill = "frtbill  2 3",
                        KeyRec = "Key Rec a ",
                        PurchaseOrder = "PO 123 ",
                        ServiceDate = DateTime.UtcNow.Date.AddDays(-1),
                        ServicerId = 1
                    });
                    DB.Invoices.Add(new Invoice()
                    {
                        SiteId = 1,
                        KeyRec = " Rec  c",
                        PurchaseOrder = "PO  abc",
                        ServiceDate = DateTime.UtcNow.Date.AddDays(-3),
                        ServicerId = 1
                    });

                    DB.SaveChanges();

                    DB.InvoiceItems.Add(new InvoiceItem()
                    {
                        InvoiceId = 1,
                        InvoiceItemId = 1,
                        ProductId = 1,
                        ServiceId = 1,
                        Quantity = 2m
                    });
                    DB.InvoiceItems.Add(new InvoiceItem()
                    {
                        InvoiceId = 1,
                        InvoiceItemId = 2,
                        ProductId = 1,
                        ServiceId = 1,
                        Quantity = 2m,
                        Comment = "Repairs needed"
                    });
                    DB.InvoiceItems.Add(new InvoiceItem()
                    {
                        InvoiceId = 1,
                        InvoiceItemId = 3,
                        ProductId = 2,
                        ServiceId = 1,
                        Quantity = 3m,
                        Comment = "Low on stock"
                    });
                    DB.InvoiceItems.Add(new InvoiceItem()
                    {
                        InvoiceId = 1,
                        InvoiceItemId = 4,
                        ProductId = 1,
                        ServiceId = 2,
                        Quantity = 4m,
                    });
                    DB.InvoiceItems.Add(new InvoiceItem()
                    {
                        InvoiceId = 1,
                        InvoiceItemId = 5,
                        ProductId = 2,
                        ServiceId = 2,
                        Quantity = 5m
                    });

                    DB.SaveChanges();
                }
            }
        }
    }
}