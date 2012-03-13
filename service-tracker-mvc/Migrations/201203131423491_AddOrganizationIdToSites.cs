namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganizationIdToSites : DbMigration
    {
        public override void Up()
        {
            AddColumn("Sites", "OrganizationId", c => c.Int(nullable: false));
            Sql("UPDATE Sites SET OrganizationId = 1;");            
            AddForeignKey("Sites", "OrganizationId", "Organizations", "OrganizationId", cascadeDelete: true);
            CreateIndex("Sites", "OrganizationId");
        }
        
        public override void Down()
        {
            DropIndex("Sites", new[] { "OrganizationId" });
            DropForeignKey("Sites", "OrganizationId", "Organizations");
            DropColumn("Sites", "OrganizationId");
        }
    }
}
