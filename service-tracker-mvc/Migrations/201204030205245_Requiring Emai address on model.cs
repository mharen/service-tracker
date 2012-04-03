namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RequiringEmaiaddressonmodel : DbMigration
    {
        public override void Up()
        {
            Sql("Update Users Set Email = 'unknown' WHERE Email IS NULL");
            AlterColumn("Users", "Email", c => c.String(nullable: false, maxLength: 254));
        }
        
        public override void Down()
        {
            AlterColumn("Users", "Email", c => c.String(maxLength: 254));
        }
    }
}
