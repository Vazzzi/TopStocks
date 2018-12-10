namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "ImagePath", c => c.String());
            DropColumn("dbo.Stocks", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "Image", c => c.Binary());
            DropColumn("dbo.Stocks", "ImagePath");
        }
    }
}
