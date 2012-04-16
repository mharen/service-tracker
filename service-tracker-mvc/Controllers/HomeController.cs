using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Entity;
using service_tracker_mvc.Data;
using service_tracker_mvc.Models;
using service_tracker_mvc.Extensions;
using System.Web.Management;

namespace service_tracker_mvc.Controllers
{
    public class HomeController : Controller
    {
        private Repo repo = new Repo();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        
        
        public MvcHtmlString CompanyNameSlug()
        {
                var profile = repo.Profiles.Single();

                return new MvcHtmlString(
                    string.Format("<a href='{0}'>{1}</a>", profile.AboutUrl, profile.Name)
                );
        }
    }

}
