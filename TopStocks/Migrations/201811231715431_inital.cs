namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inital : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "Photo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "Photo");
        }
    }
}
