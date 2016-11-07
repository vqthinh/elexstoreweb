namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdDB : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Order", "CustomerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "CustomerID", c => c.Int());
        }
    }
}
