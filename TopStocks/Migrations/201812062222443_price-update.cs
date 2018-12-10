namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class priceupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "Price_DayHighPrice", c => c.Single(nullable: false));
            AddColumn("dbo.Stocks", "Price_DayLowPrice", c => c.Single(nullable: false));
            DropColumn("dbo.Stocks", "Price_WeekHighPrice");
            DropColumn("dbo.Stocks", "Price_WeekLowPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "Price_WeekLowPrice", c => c.Single(nullable: false));
            AddColumn("dbo.Stocks", "Price_WeekHighPrice", c => c.Single(nullable: false));
            DropColumn("dbo.Stocks", "Price_DayLowPrice");
            DropColumn("dbo.Stocks", "Price_DayHighPrice");
        }
    }
}
