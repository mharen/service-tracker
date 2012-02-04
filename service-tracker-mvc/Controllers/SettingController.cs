using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace service_tracker_mvc.Controllers
{
    [Authorize(Roles = "Manager")]
    public class SettingController : Controller
    {
        //
        // GET: /Setting/

        public ActionResult Index()
        {
            return View();
        }

    }
}
