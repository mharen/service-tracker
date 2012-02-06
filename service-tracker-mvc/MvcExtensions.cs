using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using service_tracker_mvc.Data;
using service_tracker_mvc.Models;

namespace service_tracker_mvc
{
    public static class MvcExtensions
    {
        public static string ActiveTab(this HtmlHelper helper, string activeController, string[] activeActions = null, string cssClass = "ui-state-active")
        {
            string currentAction = helper.ViewContext.Controller.
            ValueProvider.GetValue("action").RawValue.ToString();
            string currentController = helper.ViewContext.Controller.
            ValueProvider.GetValue("controller").RawValue.ToString();
            string cssClassToUse = currentController == activeController &&
                                   (activeActions == null || activeActions.Contains(currentAction))
                                   ? cssClass
                                   : string.Empty;
            return cssClassToUse;
        } 

        public static SelectList ToSelectList<T>(string selectedValue)
        {
            return new SelectList(
                Enum.GetNames(typeof(T)).Select(text => new { Value = (int)Enum.Parse(typeof(T), text), Text = text }),
                "Value",
                "Text",
                selectedValue
           );
        }
        public static string VersionedContent(this UrlHelper url, string relativePath)
        {
            var UrlPath = url.Content(relativePath);
            var FilePath = HttpContext.Current.Server.MapPath(UrlPath);
            var Version = System.IO.File.GetLastWriteTime(FilePath).Ticks;

            // create querystring or append to existing
            var FormatString = UrlPath.Contains("?")
                ? "{0}&v={1}"
                : "{0}?v={1}";

            return string.Format(FormatString, UrlPath, Version);
        }

        public static string GetUserMaximumRole(this HttpContext context)
        {
            if (!context.Items.Contains(UserMaximumRoleKey))
            {
                LoadCachedUserDetails(context);
            }
            return ((RoleType)context.Items[UserMaximumRoleKey]).ToString();
        }

        public static string GetUserDisplayName(this HttpContext context)
        {
            // load the name from the database if it doesn't exist already
            if (!context.Items.Contains(UserDisplayNameKey))
            {
                LoadCachedUserDetails(context);
            }

            return context.Items[UserDisplayNameKey].ToString();
        }

        private static void LoadCachedUserDetails(HttpContext context)
        {
            using (var db = new DataContext())
            {
                var User = db.Users.Where(u => u.ClaimedIdentifier == context.User.Identity.Name)
                                   .Select(u => new { u.Email, u.RoleId, u.UserId })
                                   .Single();

                context.Items[UserDisplayNameKey] = User.Email;
                context.Items[UserMaximumRoleKey] = User.RoleId;
            }
        }

        public const string UserDisplayNameKey = "UserDisplayName";
        public const string UserMaximumRoleKey = "UserMaximumRole";
        public const string UserRole = "UserRole";

        public static string Left(this string s, int maxLength)
        {
            if (s.Length >= maxLength)
            {
                return s.Substring(0, maxLength) + "…";
            }

            return s;
        }
    }
}