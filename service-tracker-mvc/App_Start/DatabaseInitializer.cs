using System.Data.Entity;
using service_tracker_mvc.Data;

[assembly: WebActivator.PostApplicationStartMethod(typeof(service_tracker_mvc.App_Start.DatabaseInitializer), "PostStart")]

namespace service_tracker_mvc.App_Start
{
    public static class DatabaseInitializer
    {
        public static void PostStart()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
        }
    }
}
