using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using service_tracker_mvc.Models;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace service_tracker_mvc.Data
{
    public static class DataContextExtensions
    {
        private static SelectListItem AllOption = new SelectListItem { Text = "All", Value = "0" };

        public static IEnumerable<SelectListItem> ToSelectListItems(this DbSet<Organization> organizations, bool includeAllOption = false)
        {
            return ToSelectListItems(
                set: organizations,
                orderBySelector: o => o.Name,
                selectListItemTextSelector: o => o.Name,
                selectListItemValueSelector: o => o.OrganizationId.ToString(),
                includeAllOption: includeAllOption
            );
        }

        public static IEnumerable<ExtendedSelectListItem> ToSelectListItems(this DbSet<Service> services, bool includeAllOption = false)
        {
            var options = services
                 .OrderBy(s => s.Sku)
                 .ToList()
                 .Select(s => new ExtendedSelectListItem()
                 {
                     Text = s.ToString(),
                     Value = s.ServiceId.ToString(),
                     htmlAttributes = new { data_cost = s.Cost }
                 })
                 .ToList();

            if (includeAllOption)
            {
                var allOption = new ExtendedSelectListItem()
                {
                    Text = AllOption.Text,
                    Value = AllOption.Value,
                    htmlAttributes = new { data_cost = 0m }
                };

                options.Insert(0, allOption);
            }

            return options;
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this DbSet<Servicer> servicers, bool includeAllOption = false)
        {
            return ToSelectListItems(
                set: servicers,
                orderBySelector: o => o.ToString(),
                selectListItemTextSelector: o => o.Name,
                selectListItemValueSelector: o => o.ServicerId.ToString(),
                includeAllOption: includeAllOption,
                filter: GetServicerFilterForCurrentUser()
            );
        }


        public static IEnumerable<SelectListItem> ToSelectListItems(this DbSet<Site> sites, bool includeAllOption = false)
        {
            return ToSelectListItems(
                set: sites,
                orderBySelector: o => o.ToString(),
                selectListItemTextSelector: o => o.ToString(),
                selectListItemValueSelector: o => o.SiteId.ToString(),
                includeAllOption: includeAllOption,
                filter: GetSiteFilterForCurrentUser()
            );
        }

        private static IEnumerable<SelectListItem> ToSelectListItems<T>(
            DbSet<T> set,
            Func<T, string> orderBySelector,
            Func<T, string> selectListItemTextSelector,
            Func<T, string> selectListItemValueSelector,
            bool includeAllOption,
            Func<T, bool> filter = null
            ) where T : class
        {
            if (filter == null)
            {
                filter = new Func<T, bool>(t => true);
            }

            var options = set
                            .ToList() // must call this so EF doesn't try to do this stuff in SQL
                            .Where(t => filter(t))
                            .OrderBy(orderBySelector)
                            .Select(t => new SelectListItem()
                            {
                                Text = selectListItemTextSelector(t),
                                Value = selectListItemValueSelector(t)
                            })
                            .ToList();

            if (includeAllOption)
            {
                options.Insert(0, AllOption);
            }

            return options;
        }


        private static Func<Servicer, bool> GetServicerFilterForCurrentUser()
        {
            var userMaximumRole = HttpContext.Current.GetUserMaximumRole();
            Func<Servicer, bool> filter = null;

            if (userMaximumRole == RoleType.Customer)
            {
                // get all employees that have entered invoices for the given customer
                var associatedOrganizationId = HttpContext.Current.GetAssociatedOrganizationId() ?? 0;
                filter = (Servicer s) => s.Invoices.Any(i => i.Site.OrganizationId == associatedOrganizationId);
            }
            else if (userMaximumRole == RoleType.Employee)
            {
                // just return this one employee
                var associatedServicerId = HttpContext.Current.GetAssociatedServicerId() ?? 0;
                filter = (Servicer s) => s.ServicerId == associatedServicerId;
            }
            else if (userMaximumRole >= RoleType.Supervisor)
            {
                // return everyone
                filter = (Servicer s) => true;
            }
            return filter;
        }

        private static Func<Site, bool> GetSiteFilterForCurrentUser()
        {
            var userMaximumRole = HttpContext.Current.GetUserMaximumRole();
            Func<Site, bool> filter = null;

            if (userMaximumRole == RoleType.Customer)
            {
                // get just this one customer's organization
                var associatedOrganizationId = HttpContext.Current.GetAssociatedOrganizationId() ?? 0;
                filter = (Site s) => s.OrganizationId == associatedOrganizationId;
            }
            else if (userMaximumRole == RoleType.Employee)
            {
                // get all organziations that have invoices for the given employee
                var associatedServicerId = HttpContext.Current.GetAssociatedServicerId() ?? 0;
                filter = (Site s) => s.Invoices.Any(i => i.ServicerId == associatedServicerId);
            }
            else if (userMaximumRole >= RoleType.Supervisor)
            {
                // return all orgs
                filter = (Site s) => true;
            }
            return filter;
        }

        public static Func<Invoice, bool> GetInvoiceFilterForCurrentUser()
        {
            var userMaximumRole = HttpContext.Current.GetUserMaximumRole();
            Func<Invoice, bool> filter = null;

            if (userMaximumRole == RoleType.Customer)
            {
                // get just this one customer's organization's invoices
                var associatedOrganizationId = HttpContext.Current.GetAssociatedOrganizationId() ?? 0;
                filter = (Invoice i) => i.Site.OrganizationId == associatedOrganizationId;
            }
            else if (userMaximumRole == RoleType.Employee)
            {
                // get all invoices for the given employee
                var associatedServicerId = HttpContext.Current.GetAssociatedServicerId() ?? 0;
                filter = (Invoice i) => i.ServicerId == associatedServicerId;
            }
            else if (userMaximumRole >= RoleType.Supervisor)
            {
                // return all invoices
                filter = (Invoice i) => true;
            }
            return filter;
        }
    }
}