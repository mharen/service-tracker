using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace service_tracker_mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            ViewBag.Environment = ConfigurationManager.AppSettings["RuntimeEnvironment"];
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
