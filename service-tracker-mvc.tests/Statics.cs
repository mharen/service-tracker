using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using service_tracker_mvc.Classes;
using service_tracker_mvc.Models;

namespace service_tracker_mvc.tests
{
    public static class Statics
    {
        public static ICurrentUser GetFakeUser(RoleType roleType)
        {
            return new CurrentUser()
            {
                Role = roleType,
                Name = "name",
                ServicerId = 1,
                OrganizationId = 2
            };
        }

        public static Invoice GetFakeInvoice(int servicerId = 0, int organizationId = 0)
        {
            return new Invoice()
            {
                ServicerId = servicerId,
                Site = new Site()
                {
                    OrganizationId = organizationId
                }
            };
        }

        public static Servicer GetFakeServicer(int servicerId = 0, int organizationId = 0)
        {
            return new Servicer()
            {
                ServicerId = servicerId,
                Invoices = new List<Invoice>()
                {
                    GetFakeInvoice(servicerId: servicerId, organizationId: organizationId)
                }
            };
        }
    }
}
