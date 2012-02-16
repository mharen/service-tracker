using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using service_tracker_mvc.Filters;

namespace service_tracker_mvc.Controllers
{
    [RequiresAuthorizationAttribute("Manager")]
    public class SettingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
