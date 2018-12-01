namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Holdings", "UserID", c => c.Int(nullable: false));
            DropColumn("dbo.Holdings", "BuyerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Holdings", "BuyerID", c => c.Int(nullable: false));
            DropColumn("dbo.Holdings", "UserID");
        }
    }
}
