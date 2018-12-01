namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inintal1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Holdings", "BuyingTotalSum", c => c.Single(nullable: false));
            DropColumn("dbo.Holdings", "BuyingTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Holdings", "BuyingTotal", c => c.Single(nullable: false));
            DropColumn("dbo.Holdings", "BuyingTotalSum");
        }
    }
}
