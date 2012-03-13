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
                orderBySelector: o => o.Name,
                selectListItemTextSelector: o => o.Name,
                selectListItemValueSelector: o => o.ServicerId.ToString(),
                includeAllOption: includeAllOption
            );
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this DbSet<Site> sites, bool includeAllOption = false)
        {
            return ToSelectListItems(
                set: sites,
                orderBySelector: o => o.Name,
                selectListItemTextSelector: o => o.Name,
                selectListItemValueSelector: o => o.SiteId.ToString(),
                includeAllOption: includeAllOption
            );
        }

        private static IEnumerable<SelectListItem> ToSelectListItems<T>(
            DbSet<T> set,
            Expression<Func<T, string>> orderBySelector,
            Func<T, string> selectListItemTextSelector,
            Func<T, string> selectListItemValueSelector,
            bool includeAllOption) where T : class
        {
            var options = set.OrderBy(orderBySelector)
                                .ToList() // must call this so EF doesn't try to do the "new SelectListItem" stuff in SQL
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
    }
}