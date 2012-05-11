namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Addingentrydateandloginfotoinvoicesmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("Invoices", "EntryDate", c => c.DateTime(storeType: "date", nullable: true));
            Sql("UPDATE Invoices SET EntryDate = ServiceDate WHERE EntryDate IS NULL");
            AlterColumn("Invoices", "EntryDate", c => c.DateTime(nullable: false, storeType: "date"));

            AddColumn("Invoices", "LogDate", c => c.DateTime(nullable: true));
            Sql("UPDATE Invoices SET LogDate = ServiceDate WHERE LogDate IS NULL");
            AlterColumn("Invoices", "LogDate", c => c.DateTime(nullable: false));

            AddColumn("Invoices", "LogUserId", c => c.Int(nullable: true));
            // associate existing records with the manager
            Sql("UPDATE Invoices SET LogUserId = (SELECT TOP 1 UserId FROM Users WHERE RoleId = 40 ORDER BY UserId) WHERE LogUserId IS NULL");
            AlterColumn("Invoices", "LogUserId", c => c.Int(nullable: false));

            AddForeignKey("Invoices", "LogUserId", "Users", "UserId", cascadeDelete: true);
            CreateIndex("Invoices", "LogUserId");
        }

        public override void Down()
        {
            DropIndex("Invoices", new[] { "LogUserId" });
            DropForeignKey("Invoices", "LogUserId", "Users");
            DropColumn("Invoices", "LogUserId");
            DropColumn("Invoices", "LogDate");
            DropColumn("Invoices", "EntryDate");
        }
    }
}
