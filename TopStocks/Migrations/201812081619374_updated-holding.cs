namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedholding : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Holdings", "CurrentPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Holdings", "CurrentPrice", c => c.Single(nullable: false));
        }
    }
}
