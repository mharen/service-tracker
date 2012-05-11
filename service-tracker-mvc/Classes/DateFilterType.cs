using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Classes
{
    public enum DateFilterType
    {
        [Display(Name = "Service Date Range")]
        ServiceDate,
        [Display(Name = "Entry Date Range")]
        EntryDate
    }
}