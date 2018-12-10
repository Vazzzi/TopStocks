namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inintal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Holdings", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Holdings", "Quantity");
        }
    }
}
