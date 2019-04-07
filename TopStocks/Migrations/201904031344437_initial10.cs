namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "Category", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "Category");
        }
    }
}
