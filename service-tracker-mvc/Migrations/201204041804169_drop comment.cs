namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class dropcomment : DbMigration
    {
        public override void Up()
        {
            DropColumn("InvoiceItems", "Comment");
        }
        
        public override void Down()
        {
            AddColumn("InvoiceItems", "Comment", c => c.String(maxLength: 250));
        }
    }
}
