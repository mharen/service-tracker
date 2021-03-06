﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using service_tracker_mvc.Data;
using service_tracker_mvc.Models;
using System.Web.Caching;
using service_tracker_mvc.Classes;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace service_tracker_mvc.Extensions
{
    public static class MvcExtensions
    {
        public static string ToHtmlDateInputString(this DateTime? value)
        {
            if (value != null && value != DateTime.MinValue)
            {
                return value.Value.ToString("yyyy-MM-dd");
            }

            return "";
        }

        // via: http://stackoverflow.com/a/968873/29
        private static object sync = new object();
        public const int DefaultCacheExpiration = 20;

        /// <summary>
        /// Allows Caching of typed data
        /// </summary>
        /// <example><![CDATA[
        /// var user = HttpRuntime
        ///   .Cache
        ///   .GetOrStore<User>(
        ///      string.Format("User{0}", _userId), 
        ///      () => Repository.GetUser(_userId));
        ///
        /// ]]></example>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">calling object</param>
        /// <param name="key">Cache key</param>
        /// <param name="generator">Func that returns the object to store in cache</param>
        /// <returns></returns>
        /// <remarks>Uses a default cache expiration period as defined in <see cref="CacheExtensions.DefaultCacheExpiration"/></remarks>
        public static T GetOrStore<T>(this Cache cache, string key, Func<T> generator)
        {
            return cache.GetOrStore(key, (cache[key] == null && generator != null) ? generator() : default(T), DefaultCacheExpiration);
        }

        /// <summary>
        /// Allows Caching of typed data
        /// </summary>
        /// <example><![CDATA[
        /// var user = HttpRuntime
        ///   .Cache
        ///   .GetOrStore<User>(
        ///      string.Format("User{0}", _userId), 
        ///      () => Repository.GetUser(_userId));
        ///
        /// ]]></example>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">calling object</param>
        /// <param name="key">Cache key</param>
        /// <param name="generator">Func that returns the object to store in cache</param>
        /// <param name="expireInMinutes">Time to expire cache in minutes</param>
        /// <returns></returns>
        public static T GetOrStore<T>(this Cache cache, string key, Func<T> generator, double expireInMinutes)
        {
            return cache.GetOrStore(key, (cache[key] == null && generator != null) ? generator() : default(T), expireInMinutes);
        }

        /// <summary>
        /// Allows Caching of typed data
        /// </summary>
        /// <example><![CDATA[
        /// var user = HttpRuntime
        ///   .Cache
        ///   .GetOrStore<User>(
        ///      string.Format("User{0}", _userId),_userId));
        ///
        /// ]]></example>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">calling object</param>
        /// <param name="key">Cache key</param>
        /// <param name="obj">Object to store in cache</param>
        /// <returns></returns>
        /// <remarks>Uses a default cache expiration period as defined in <see cref="CacheExtensions.DefaultCacheExpiration"/></remarks>
        public static T GetOrStore<T>(this Cache cache, string key, T obj)
        {
            return cache.GetOrStore(key, obj, DefaultCacheExpiration);
        }

        /// <summary>
        /// Allows Caching of typed data
        /// </summary>
        /// <example><![CDATA[
        /// var user = HttpRuntime
        ///   .Cache
        ///   .GetOrStore<User>(
        ///      string.Format("User{0}", _userId), 
        ///      () => Repository.GetUser(_userId));
        ///
        /// ]]></example>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">calling object</param>
        /// <param name="key">Cache key</param>
        /// <param name="obj">Object to store in cache</param>
        /// <param name="expireInMinutes">Time to expire cache in minutes</param>
        /// <returns></returns>
        public static T GetOrStore<T>(this Cache cache, string key, T obj, double expireInMinutes)
        {
            var result = cache[key];

            if (result == null)
            {

                lock (sync)
                {
                    if (result == null)
                    {
                        result = obj != null ? obj : default(T);
                        cache.Insert(key, result, null, DateTime.Now.AddMinutes(expireInMinutes), Cache.NoSlidingExpiration);
                    }
                }
            }

            return (T)result;

        }

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

        public static SelectList ToSelectList<T>(string selectedValue) where T : struct, IConvertible
        {
            return new SelectList(
                Enum.GetNames(typeof(T)).Select(text => new { Value = (int)Enum.Parse(typeof(T), text), Text = text }),
                "Value",
                "Text",
                selectedValue
           );
        }

        public static IHtmlString DropDownListForEnum<TModel, TProperty>(
                this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression,
                string optionLabel,
                object htmlAttributes
            )
        {
            if (!typeof(TProperty).IsEnum)
            {
                throw new Exception("This helper can be used only with enum types");
            }

            var enumType = typeof(TProperty);
            var fields = enumType.GetFields(
                BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public
            );
            var values = Enum.GetValues(enumType).OfType<TProperty>();
            var items =
                from value in values
                from field in fields
                let descriptionAttribute = field
                    .GetCustomAttributes(
                        typeof(DisplayAttribute), true
                    )
                    .OfType<DisplayAttribute>()
                    .FirstOrDefault()
                let displayName = (descriptionAttribute != null)
                    ? descriptionAttribute.Name
                    : value.ToString()
                where value.ToString() == field.Name
                select new { Id = value, Name = displayName };

            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumObj = metadata;
            var selectList = new SelectList(items, "Id", "Name", metadata.Model);
            
            return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
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

        public static ICurrentUser CurrentUser(this HttpContext context, ICurrentUser user = null)
        {
            return DataContextExtensions.LoadUser(user);
        }

        public static string Left(this string s, int maxLength)
        {
            if (s != null && s.Length >= maxLength)
            {
                return s.Substring(0, maxLength) + "…";
            }

            return s;
        }

        // via http://stackoverflow.com/a/4360017/29
        public static MvcHtmlString Concat(params MvcHtmlString[] items)
        {
            var sb = new StringBuilder();
            foreach (var item in items.Where(i => i != null))
                sb.Append(item.ToHtmlString());
            return MvcHtmlString.Create(sb.ToString());
        }

        public static string PrettifyUrl(this string url, int maxLength = 15)
        {
            if (url == null) { return null; }

            return url.Replace("https://", "").Replace("http://", "").Replace("www.", "").Left(maxLength);
        }

        // 99% via http://support.appharbor.com/kb/getting-started/workaround-for-generating-absolute-urls-without-port-number
        public static string ToPublicUrl(this UrlHelper urlHelper, string relativeUri)
        {
            var httpContext = urlHelper.RequestContext.HttpContext;

            var uriBuilder = new UriBuilder
            {
                Host = httpContext.Request.Url.Host,
                Path = "/",
                Port = 80,
                Scheme = "http",
            };

            if (httpContext.Request.IsLocal)
            {
                uriBuilder.Port = httpContext.Request.Url.Port;
            }

            return new Uri(uriBuilder.Uri, relativeUri).AbsoluteUri;
        }
    }
}