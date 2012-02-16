﻿using System.Web.Mvc;
using service_tracker_mvc.Filters;


namespace MvcAuthorize.Tests
{
	public class FooController : Controller
	{
        [RequiresAuthorizationAttribute("Guest")]
        public ActionResult PermisiveAction()
		{
			return View();
		}

        [RequiresAuthorizationAttribute("Manager")]
		public ActionResult MoreRestrictedAction()
		{
			return View();
		}
	}
}