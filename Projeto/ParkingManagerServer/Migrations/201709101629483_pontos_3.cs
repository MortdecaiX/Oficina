namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pontos_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstacionamentoModels", "ImagemBase64", c => c.String());
            DropColumn("dbo.EstacionamentoModels", "Imagem");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EstacionamentoModels", "Imagem", c => c.String());
            DropColumn("dbo.EstacionamentoModels", "ImagemBase64");
        }
    }
}
