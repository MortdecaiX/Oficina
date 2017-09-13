namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PontoAPonto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PontoModels", "PontoModel_Id", "dbo.PontoModels");
            DropIndex("dbo.PontoModels", new[] { "PontoModel_Id" });
            CreateTable(
                "dbo.PontoPonto",
                c => new
                    {
                        PontoPaiRefId = c.Long(nullable: false),
                        PontoFilhoRefId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.PontoPaiRefId, t.PontoFilhoRefId })
                .ForeignKey("dbo.PontoModels", t => t.PontoPaiRefId)
                .ForeignKey("dbo.PontoModels", t => t.PontoFilhoRefId)
                .Index(t => t.PontoPaiRefId)
                .Index(t => t.PontoFilhoRefId);
            
            DropColumn("dbo.PontoModels", "PontoModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PontoModels", "PontoModel_Id", c => c.Long());
            DropForeignKey("dbo.PontoPonto", "PontoFilhoRefId", "dbo.PontoModels");
            DropForeignKey("dbo.PontoPonto", "PontoPaiRefId", "dbo.PontoModels");
            DropIndex("dbo.PontoPonto", new[] { "PontoFilhoRefId" });
            DropIndex("dbo.PontoPonto", new[] { "PontoPaiRefId" });
            DropTable("dbo.PontoPonto");
            CreateIndex("dbo.PontoModels", "PontoModel_Id");
            AddForeignKey("dbo.PontoModels", "PontoModel_Id", "dbo.PontoModels", "Id");
        }
    }
}
