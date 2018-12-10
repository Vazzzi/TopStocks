namespace TopStocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Holdings", "StockID", "dbo.Stocks");
            DropIndex("dbo.Holdings", new[] { "StockID" });
            RenameColumn(table: "dbo.Holdings", name: "StockID", newName: "Stock_ID");
            AddColumn("dbo.Holdings", "StockName", c => c.String());
            AlterColumn("dbo.Holdings", "Stock_ID", c => c.Int());
            CreateIndex("dbo.Holdings", "Stock_ID");
            AddForeignKey("dbo.Holdings", "Stock_ID", "dbo.Stocks", "ID");
            DropColumn("dbo.Holdings", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Holdings", "UserID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Holdings", "Stock_ID", "dbo.Stocks");
            DropIndex("dbo.Holdings", new[] { "Stock_ID" });
            AlterColumn("dbo.Holdings", "Stock_ID", c => c.Int(nullable: false));
            DropColumn("dbo.Holdings", "StockName");
            RenameColumn(table: "dbo.Holdings", name: "Stock_ID", newName: "StockID");
            CreateIndex("dbo.Holdings", "StockID");
            AddForeignKey("dbo.Holdings", "StockID", "dbo.Stocks", "ID", cascadeDelete: true);
        }
    }
}
