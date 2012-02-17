using System.Data.Entity;
using service_tracker_mvc.Data;

[assembly: WebActivator.PreApplicationStartMethod(typeof(service_tracker_mvc.App_Start.DatabaseInitializer), "Start")]

namespace service_tracker_mvc.App_Start
{
    public static class DatabaseInitializer
    {
        public static void Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
            Utilites.SeedData();
        }
    }
}
