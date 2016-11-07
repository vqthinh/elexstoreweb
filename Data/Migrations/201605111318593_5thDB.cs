namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5thDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Order");
            AddForeignKey("dbo.Products", "CategoryID", "dbo.Categories", "ID");
            AddForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products", "ID");
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers", "ID");
            AddForeignKey("dbo.OrderDetails", "OrderID", "dbo.Order", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Order");
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            AddForeignKey("dbo.OrderDetails", "OrderID", "dbo.Order", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Products", "CategoryID", "dbo.Categories", "ID", cascadeDelete: true);
        }
    }
}
