namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VagaModels", "PontoModel_Id", "dbo.PontoModels");
            AddForeignKey("dbo.VagaModels", "PontoModel_Id", "dbo.PontoModels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VagaModels", "PontoModel_Id", "dbo.PontoModels");
            AddForeignKey("dbo.VagaModels", "PontoModel_Id", "dbo.PontoModels", "Id");
        }
    }
}
