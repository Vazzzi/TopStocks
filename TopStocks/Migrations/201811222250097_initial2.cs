namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "Name", c => c.String());
            DropColumn("dbo.Stocks", "Photo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "Photo", c => c.String());
            DropColumn("dbo.Stocks", "Name");
        }
    }
}
