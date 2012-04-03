namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    using System;
    
    public partial class cleaningupinvitations : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "InvitationCode", c => c.String(maxLength: 80));
            DropColumn("Users", "InvitationId");
        }
        
        public override void Down()
        {
            AddColumn("Users", "InvitationId", c => c.Guid(nullable: false, defaultValue: Guid.Empty));
            DropColumn("Users", "InvitationCode");
        }
    }
}
