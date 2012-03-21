using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace service_tracker_mvc.Classes
{
    public class PairEditor
    {
        public MvcHtmlString Label { get; set; }
        public MvcHtmlString Editor { get; set; }
        public MvcHtmlString Validation { get; set; }
    }
}