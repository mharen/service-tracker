namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddSupervisorRoleType : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Users SET RoleId = (RoleId * 10) % 100");
        }
        
        public override void Down()
        {
            Sql("UPDATE Users SET RoleId = (RoleId / 10)");
        }
    }
}
