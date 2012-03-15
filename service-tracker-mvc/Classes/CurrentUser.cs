using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using service_tracker_mvc.Models;
using service_tracker_mvc.Data;

namespace service_tracker_mvc.Classes
{
    public class CurrentUser : ICurrentUser
    {
        public int UserId { get; set; }
        public RoleType Role { get; set; }
        public string Name { get; set; }
        public int? ServicerId { get; set; }
        public int? OrganizationId { get; set; }
    }
}