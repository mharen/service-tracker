using System;
using System.Net;
using System.Web.Mvc;

namespace service_tracker_mvc.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        public ActionResult ServerError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
        }

        public ActionResult Unauthorized(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
            }

            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return View();
        }
    }
}