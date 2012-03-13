namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class SplitCustomerIntoOrganizationAndSite : DbMigration
    {
        public override void Up()
        {
            AddColumn("Invoices", "SiteId", c => c.Int(nullable: false));
            CreateIndex("Invoices", "SiteId");

            Sql("SET IDENTITY_INSERT Organizations ON;");
            Sql("INSERT INTO Organizations (OrganizationId, Name, Code) VALUES (1, 'Default Organization', 'org')");
            Sql("SET IDENTITY_INSERT Organizations OFF;");

            Sql("SET IDENTITY_INSERT Sites ON;");
            Sql("INSERT INTO Sites (SiteId, Name, Address, OrganizationId) SELECT CustomerId, Name, Address, 1 FROM Customers");
            Sql("SET IDENTITY_INSERT Sites OFF;");

            Sql("UPDATE Invoices SET SiteId = CustomerId");

            DropForeignKey("Invoices", "Invoice_Customer");
            DropColumn("Invoices", "CustomerId");
            DropColumn("Invoices", "Site_SiteId");
            DropTable("Customers");
        }
        
        public override void Down()
        {
            CreateTable(
                "Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Address = c.String(maxLength: 250),
                        VendorNumber = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            AddColumn("Invoices", "Site_SiteId", c => c.Int());
            AddColumn("Invoices", "CustomerId", c => c.Int(nullable: false));
            DropIndex("Invoices", new[] { "SiteId" });
            DropForeignKey("Invoices", "SiteId", "Sites");
            DropColumn("Invoices", "SiteId");
            CreateIndex("Invoices", "Site_SiteId");
            CreateIndex("Invoices", "CustomerId");
            AddForeignKey("Invoices", "Site_SiteId", "Sites", "SiteId");
            AddForeignKey("Invoices", "CustomerId", "Customers", "CustomerId", cascadeDelete: true);
        }
    }
}
