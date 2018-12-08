namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedvirtint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Holdings", "Stock_ID", "dbo.Stocks");
            DropIndex("dbo.Holdings", new[] { "Stock_ID" });
            RenameColumn(table: "dbo.Holdings", name: "Stock_ID", newName: "StockID");
            AlterColumn("dbo.Holdings", "StockID", c => c.Int(nullable: false));
            CreateIndex("dbo.Holdings", "StockID");
            AddForeignKey("dbo.Holdings", "StockID", "dbo.Stocks", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Holdings", "StockID", "dbo.Stocks");
            DropIndex("dbo.Holdings", new[] { "StockID" });
            AlterColumn("dbo.Holdings", "StockID", c => c.Int());
            RenameColumn(table: "dbo.Holdings", name: "StockID", newName: "Stock_ID");
            CreateIndex("dbo.Holdings", "Stock_ID");
            AddForeignKey("dbo.Holdings", "Stock_ID", "dbo.Stocks", "ID");
        }
    }
}
