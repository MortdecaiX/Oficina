namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustesEntidades1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VagaModels", "Responsavel_Id", "dbo.UsuarioModels");
            DropIndex("dbo.VagaModels", new[] { "Responsavel_Id" });
            DropColumn("dbo.VagaModels", "Responsavel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VagaModels", "Responsavel_Id", c => c.Long());
            CreateIndex("dbo.VagaModels", "Responsavel_Id");
            AddForeignKey("dbo.VagaModels", "Responsavel_Id", "dbo.UsuarioModels", "Id");
        }
    }
}
