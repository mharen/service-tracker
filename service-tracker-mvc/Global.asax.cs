using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using service_tracker_mvc.Data;
using service_tracker_mvc.Models;
using System.Data.Entity;
using Devtalk.EF.CodeFirst;

namespace service_tracker_mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //Database.SetInitializer(new SeedDataInitializer<DataContext>());
            Database.SetInitializer(new DontDropDbJustCreateTablesIfModelChanged<DataContext>());
        }

        private void SeedData()
        {
            using (var DB = new DataContext())
            {
                if (!DB.Customers.Any())
                {
                    DB.Customers.Add(new Customer() { Name = "Customer A", Address = "123 Main Street", VendorNumber = "123abc" });
                    DB.Customers.Add(new Customer() { Name = "Customer B", Address = "456 High Street", VendorNumber = "456def" });
                    DB.Customers.Add(new Customer() { Name = "Customer C", Address = "789 North Street", VendorNumber = "789ghi" });
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
                        CustomerId = 1,
                        FrtBill = "frtbill 1  3",
                        KeyRec = "Key Rec  b c",
                        PurchaseOrder = "PO 123 abc",
                        ServiceDate = DateTime.UtcNow.Date,
                        ServicerId = 1
                    });
                    DB.Invoices.Add(new Invoice()
                    {
                        CustomerId = 1,
                        FrtBill = "frtbill 1 2  4",
                        KeyRec = "Key a b d",
                        PurchaseOrder = " 123 abcd",
                        ServiceDate = DateTime.UtcNow.Date.AddDays(-1),
                        ServicerId = 1
                    });
                    DB.Invoices.Add(new Invoice()
                    {
                        CustomerId = 1,
                        FrtBill = "frtbill  2 3",
                        KeyRec = "Key Rec a ",
                        PurchaseOrder = "PO 123 ",
                        ServiceDate = DateTime.UtcNow.Date.AddDays(-1),
                        ServicerId = 1
                    });
                    DB.Invoices.Add(new Invoice()
                    {
                        CustomerId = 1,
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