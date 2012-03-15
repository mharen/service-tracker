using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using service_tracker_mvc.Models;

namespace service_tracker_mvc.Classes
{
    public interface ICurrentUser
    {
        int UserId { get; set; }
        RoleType Role { get; set; }
        string Name { get; set; }
        int? ServicerId { get; set; }
        int? OrganizationId { get; set; }
    }
}
