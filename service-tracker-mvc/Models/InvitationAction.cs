using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace service_tracker_mvc.Models
{
    public enum InvitationAction
    {
        Unknown = 0,
        Created = 1,
        Sent = 2,
        Accepted = 3,
        Deleted = 4
    }
}