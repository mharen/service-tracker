using System.Data.Entity;
using Devtalk.EF.CodeFirst;
using service_tracker_mvc.Data;

[assembly: WebActivator.PreApplicationStartMethod(typeof(service_tracker_mvc.App_Start.DontDropDbJustCreateTablesIfModelChangedStart), "Start")]

namespace service_tracker_mvc.App_Start {
    public static class DontDropDbJustCreateTablesIfModelChangedStart {
        public static void Start() {
            using (var DB = new DataContext())
            {
                DB.Database.CreateIfNotExists();
            }
            Database.SetInitializer(new DontDropDbJustCreateTablesIfModelChanged<DataContext>());
            Utilites.SeedData();
        }
    }
}
