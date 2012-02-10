using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace service_tracker_mvc
{
    // via http://stackoverflow.com/a/876581/29
    public class RequiresAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly bool _authorize;
        private readonly string _minimumRole;

        public RequiresAuthorizationAttribute()
        {
            _authorize = true;
            _minimumRole = "Manager";
        }

        public RequiresAuthorizationAttribute(bool authorize):base()
        {
            _authorize = authorize;
        }
        
        public RequiresAuthorizationAttribute(string minimumRole = "Manager"):base()
        {
            _minimumRole = minimumRole;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var overridingAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(RequiresAuthorizationAttribute), false);

            if (overridingAttributes.Length > 0 && overridingAttributes[0] as RequiresAuthorizationAttribute != null && !((RequiresAuthorizationAttribute)overridingAttributes[0])._authorize)
                return;

            if (_authorize)
            {
                //redirect if not authenticated
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated || !filterContext.HttpContext.User.IsInRole(_minimumRole))
                {
                    //use the current url for the redirect
                    var redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;

                    //send them off to the login page
                    //var redirectUrl = string.Format("?RedirectUrl={0}", redirectOnSuccess);
                    var loginUrl = UrlHelper.GenerateUrl("Default", "Login", "User", new RouteValueDictionary(new { returnUrl = redirectOnSuccess }), RouteTable.Routes, filterContext.RequestContext, false);
                    filterContext.HttpContext.Response.Redirect(loginUrl, true);
                }
            }
        }
    }
}