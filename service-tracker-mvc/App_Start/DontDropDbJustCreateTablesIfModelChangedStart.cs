using System.Data.Entity;
using Devtalk.EF.CodeFirst;
using service_tracker_mvc.Data;

[assembly: WebActivator.PreApplicationStartMethod(typeof(service_tracker_mvc.App_Start.DontDropDbJustCreateTablesIfModelChangedStart), "Start")]

namespace service_tracker_mvc.App_Start {
    public static class DontDropDbJustCreateTablesIfModelChangedStart {
        public static void Start() {
            // Uncomment this line and replace CONTEXT_NAME with the name of your DbContext if you are 
            // using your DbContext to create and manage your database
            new LogEvent("In app start");
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());
            //Database.SetInitializer(new DontDropDbJustCreateTablesIfModelChanged<DataContext>());
        }
    }
}
