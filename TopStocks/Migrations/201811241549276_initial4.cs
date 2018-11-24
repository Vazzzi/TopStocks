namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stocks", "Price_CurrentPrice", c => c.Single(nullable: false));
            AlterColumn("dbo.Stocks", "Price_WeekHighPrice", c => c.Single(nullable: false));
            AlterColumn("dbo.Stocks", "Price_WeekLowPrice", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stocks", "Price_WeekLowPrice", c => c.Int(nullable: false));
            AlterColumn("dbo.Stocks", "Price_WeekHighPrice", c => c.Int(nullable: false));
            AlterColumn("dbo.Stocks", "Price_CurrentPrice", c => c.Int(nullable: false));
        }
    }
}
