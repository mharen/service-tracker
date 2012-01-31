using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using service_tracker_mvc.Data;

namespace service_tracker_mvc
{
    public static class MvcExtensions
    {
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

        public static string GetUserDisplayName(this HttpContext context)
        {
            // load the name from the database if it doesn't exist already
            if (!context.Items.Contains(UserDisplayNameKey))
            {
                using (var db = new DataContext())
                {
                    context.Items[UserDisplayNameKey]
                        = db.Users.Single(u => u.ClaimedIdentifier == context.User.Identity.Name).Email;
                }
            }

            return context.Items[UserDisplayNameKey].ToString();
        }
        public const string UserDisplayNameKey = "UserDisplayName";
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