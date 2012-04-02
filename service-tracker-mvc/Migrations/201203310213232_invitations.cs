namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class invitations : DbMigration
    {
        public override void Up()
        {
            DropColumn("Users", "InvitationCode");
        }
        
        public override void Down()
        {
        }
    }
}
