using System.Data.Entity;
using Devtalk.EF.CodeFirst;
using service_tracker_mvc.Data;

[assembly: WebActivator.PreApplicationStartMethod(typeof(service_tracker_mvc.App_Start.DontDropDbJustCreateTablesIfModelChangedStart), "Start")]

namespace service_tracker_mvc.App_Start
{
    public static class DontDropDbJustCreateTablesIfModelChangedStart
    {
        public static void Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
            Utilites.SeedData();
        }
    }
}
