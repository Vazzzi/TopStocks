namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Holdings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BuyerID = c.Int(nullable: false),
                        BuyingDate = c.DateTime(nullable: false),
                        BuyingPrice = c.Single(nullable: false),
                        CurrentPrice = c.Single(nullable: false),
                        StockID = c.Int(nullable: false),
                        Buyer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Buyer_Id)
                .ForeignKey("dbo.Stocks", t => t.StockID, cascadeDelete: true)
                .Index(t => t.StockID)
                .Index(t => t.Buyer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Holdings", "StockID", "dbo.Stocks");
            DropForeignKey("dbo.Holdings", "Buyer_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Holdings", new[] { "Buyer_Id" });
            DropIndex("dbo.Holdings", new[] { "StockID" });
            DropTable("dbo.Holdings");
        }
    }
}
