namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Holdings", "BuyingTotal", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Holdings", "BuyingTotal");
        }
    }
}
