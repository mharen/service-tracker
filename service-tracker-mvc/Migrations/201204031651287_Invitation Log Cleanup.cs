namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InvitationLogCleanup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("InvitationLogs", "User_UserId", "Users");
            DropIndex("InvitationLogs", new[] { "User_UserId" });
            DropTable("InvitationLogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "InvitationLogs",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        LogDate = c.DateTime(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateIndex("InvitationLogs", "User_UserId");
            AddForeignKey("InvitationLogs", "User_UserId", "Users", "UserId");
        }
    }
}
