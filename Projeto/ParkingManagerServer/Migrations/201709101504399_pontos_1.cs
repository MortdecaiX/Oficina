namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pontos_1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PontoModels", new[] { "PontoModel_Id" });
            AlterColumn("dbo.PontoModels", "PontoModel_Id", c => c.Long());
            CreateIndex("dbo.PontoModels", "PontoModel_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PontoModels", new[] { "PontoModel_Id" });
            AlterColumn("dbo.PontoModels", "PontoModel_Id", c => c.Long(nullable: false));
            CreateIndex("dbo.PontoModels", "PontoModel_Id");
        }
    }
}
