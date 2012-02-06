using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using MvcMiniProfiler;
using MvcMiniProfiler.MVCHelpers;
using Microsoft.Web.Infrastructure;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using MvcMiniProfiler.Data.EntityFramework;
//using MvcMiniProfiler.Data.Linq2Sql;

[assembly: WebActivator.PreApplicationStartMethod(
	typeof(service_tracker_mvc.App_Start.MiniProfilerPackage), "PreStart")]

[assembly: WebActivator.PostApplicationStartMethod(
	typeof(service_tracker_mvc.App_Start.MiniProfilerPackage), "PostStart")]


namespace service_tracker_mvc.App_Start 
{
    public static class MiniProfilerPackage
    {
        public static void PreStart()
        {
            //TODO: Non SQL Server based installs can use other formatters like: new MvcMiniProfiler.SqlFormatters.InlineFormatter()
            MiniProfiler.Settings.SqlFormatter = new MvcMiniProfiler.SqlFormatters.SqlServerFormatter();

			//TODO: To profile a standard DbConnection: 
			// var profiled = new ProfiledDbConnection(cnn, MiniProfiler.Current);

            //TODO: If you are profiling EF code first try: 
            // Uncomment this to let out the boom. Waiting for an update to the pkg so I can re-enable the awesomesauce
			//MiniProfilerEF.Initialize();

            //Make sure the MiniProfiler handles BeginRequest and EndRequest
            DynamicModuleUtility.RegisterModule(typeof(MiniProfilerStartupModule));

            //Setup profiler for Controllers via a Global ActionFilter
            GlobalFilters.Filters.Add(new ProfilingActionFilter());
        }

        public static void PostStart()
        {
            // Intercept ViewEngines to profile all partial views and regular views.
            // If you prefer to insert your profiling blocks manually you can comment this out
            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (var item in copy)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }
        }
    }

    public class MiniProfilerStartupModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) =>
            {
                var request = ((HttpApplication)sender).Request;
                //TODO: By default only local requests are profiled, optionally you can set it up
                //  so authenticated users are always profiled
                if (request.IsLocal) { MiniProfiler.Start(); }
            };


            // TODO: You can control who sees the profiling information
            //context.AuthenticateRequest += (sender, e) =>
            //{
            //    if (HttpContext.Current.User == null || !HttpContext.Current.User.IsInRole("Administrator"))
            //    {
            //        MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
            //    }
            //};

            context.EndRequest += (sender, e) =>
            {
                MiniProfiler.Stop();
            };
        }

        public void Dispose() { }
    }
}

