namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Order", new[] { "CustomerID" });
            AlterColumn("dbo.Order", "CustomerID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Order", "CustomerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Order", "CustomerID");
            AddForeignKey("dbo.Order", "CustomerID", "dbo.Customers", "CustomerID");
        }
    }
}
