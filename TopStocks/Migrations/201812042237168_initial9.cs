namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Holdings", "BuyingValue", c => c.Single(nullable: false));
            DropColumn("dbo.Holdings", "BuyingTotalSum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Holdings", "BuyingTotalSum", c => c.Single(nullable: false));
            DropColumn("dbo.Holdings", "BuyingValue");
        }
    }
}
