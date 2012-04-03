namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Addingemailinfotoprofilestable : DbMigration
    {
        public override void Up()
        {
            AddColumn("Profiles", "EmailFromAddress", c => c.String(maxLength: 100));
            AddColumn("Profiles", "EmailBccAddress", c => c.String(maxLength: 100));
            Sql("Update Profiles Set EmailFromAddress='info@hickmanassembly.com', EmailBccAddress='info@hickmanassembly.com'");
        }
        
        public override void Down()
        {
            DropColumn("Profiles", "EmailBccAddress");
            DropColumn("Profiles", "EmailFromAddress");
        }
    }
}
