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

        private static IEnumerable<SelectListItem> ToSelectListItems<T>(
            DbSet<T> set,
            Expression<Func<T, string>> orderBySelector,
            Func<T, string> selectListItemTextSelector,
            Func<T, string> selectListItemValueSelector,
            bool includeAllOption) where T: class
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