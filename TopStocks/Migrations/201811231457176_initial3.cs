namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "Image");
        }
    }
}
