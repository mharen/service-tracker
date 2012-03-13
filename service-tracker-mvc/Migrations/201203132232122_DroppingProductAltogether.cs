namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DroppingProductAltogether : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("InvoiceItems", "ProductId", "Products");
            DropIndex("InvoiceItems", new[] { "ProductId" });
            DropColumn("InvoiceItems", "ProductId");
            DropTable("Products");
        }
        
        public override void Down()
        {
            CreateTable(
                "Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Manufacturer = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductId);
            
            AddColumn("InvoiceItems", "ProductId", c => c.Int());
            CreateIndex("InvoiceItems", "ProductId");
            AddForeignKey("InvoiceItems", "ProductId", "Products", "ProductId");
        }
    }
}
