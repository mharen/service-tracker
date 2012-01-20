using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Entity;
using service_tracker_mvc.Data;
using service_tracker_mvc.Models;

namespace service_tracker_mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            ViewBag.Environment = ConfigurationManager.AppSettings["RuntimeEnvironment"];

            using (var DB = new DataContext())
            {
                var Customer = new Customer() { Name = "Acme", Address = "123 Main St.", VendorNumber = "123" };
                DB.Customers.Add(Customer);

                DB.SaveChanges();
            }
   
            using (var DB = new DataContext())
            {
                var Customers = DB.Customers.ToList();
                return View(Customers);
            }
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
