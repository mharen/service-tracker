namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class MarkingSiteNameRequired : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Sites SET Name = '(unknown)' WHERE Name IS NULL");
            AlterColumn("Sites", "Name", c => c.String(nullable: false, maxLength: 50));

            Sql("UPDATE Organizations SET Name = '(unknown)' WHERE Name IS NULL");
            AlterColumn("Organizations", "Name", c => c.String(nullable: false, maxLength: 50));

            Sql("UPDATE Organizations SET Code = 'empty' WHERE Code IS NULL");
            AlterColumn("Organizations", "Code", c => c.String(nullable: false, maxLength: 6));

            Sql("UPDATE Profiles SET Name = '(unknown)' WHERE Name IS NULL");
            AlterColumn("Profiles", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("Profiles", "Name", c => c.String(maxLength: 50));
            AlterColumn("Organizations", "Code", c => c.String(maxLength: 6));
            AlterColumn("Organizations", "Name", c => c.String(maxLength: 50));
            AlterColumn("Sites", "Name", c => c.String(maxLength: 50));
        }
    }
}
