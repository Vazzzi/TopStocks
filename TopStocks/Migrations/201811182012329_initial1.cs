namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "Price_CurrentPrice", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "Price_WeekHighPrice", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "Price_WeekLowPrice", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "Photo", c => c.String());
            DropColumn("dbo.Stocks", "StockCurrentPrice");
            DropColumn("dbo.Stocks", "WeekHigh");
            DropColumn("dbo.Stocks", "WeekLow");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "WeekLow", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "WeekHigh", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "StockCurrentPrice", c => c.Int(nullable: false));
            DropColumn("dbo.Stocks", "Photo");
            DropColumn("dbo.Stocks", "Price_WeekLowPrice");
            DropColumn("dbo.Stocks", "Price_WeekHighPrice");
            DropColumn("dbo.Stocks", "Price_CurrentPrice");
        }
    }
}
