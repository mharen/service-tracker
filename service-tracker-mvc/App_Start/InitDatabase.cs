using System.Data.Entity;
using Devtalk.EF.CodeFirst;
using service_tracker_mvc.Data;
using System.Linq;
using service_tracker_mvc.Models;

[assembly: WebActivator.PreApplicationStartMethod(typeof(service_tracker_mvc.App_Start.InitDatabase), "Start")]

namespace service_tracker_mvc.App_Start {
    public static class InitDatabase {
        public static void Start() {
            // Uncomment this line and replace CONTEXT_NAME with the name of your DbContext if you are 
            // using your DbContext to create and manage your database
            new LogEvent("In app start");
            Database.SetInitializer(new SeedDataInitializer<DataContext>());
            //Database.SetInitializer(new DontDropDbJustCreateTablesIfModelChanged<DataContext>());

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
            }
        }
    }
}
