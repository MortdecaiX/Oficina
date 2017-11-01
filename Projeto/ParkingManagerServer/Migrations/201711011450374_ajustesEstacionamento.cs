namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustesEstacionamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstacionamentoModels", "SWBoundImagem_Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "SWBoundImagem_Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "SWBoundImagem_Altitude", c => c.Double(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "NEBoundImagem_Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "NEBoundImagem_Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "NEBoundImagem_Altitude", c => c.Double(nullable: false));
            DropColumn("dbo.EstacionamentoModels", "ImagemAltura");
            DropColumn("dbo.EstacionamentoModels", "ImagemLargura");
            DropColumn("dbo.EstacionamentoModels", "ImagemRotacao");
            DropColumn("dbo.EstacionamentoModels", "LocalizacaoImagem_Latitude");
            DropColumn("dbo.EstacionamentoModels", "LocalizacaoImagem_Longitude");
            DropColumn("dbo.EstacionamentoModels", "LocalizacaoImagem_Altitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EstacionamentoModels", "LocalizacaoImagem_Altitude", c => c.Double(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "LocalizacaoImagem_Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "LocalizacaoImagem_Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "ImagemRotacao", c => c.Single(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "ImagemLargura", c => c.Long(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "ImagemAltura", c => c.Long(nullable: false));
            DropColumn("dbo.EstacionamentoModels", "NEBoundImagem_Altitude");
            DropColumn("dbo.EstacionamentoModels", "NEBoundImagem_Longitude");
            DropColumn("dbo.EstacionamentoModels", "NEBoundImagem_Latitude");
            DropColumn("dbo.EstacionamentoModels", "SWBoundImagem_Altitude");
            DropColumn("dbo.EstacionamentoModels", "SWBoundImagem_Longitude");
            DropColumn("dbo.EstacionamentoModels", "SWBoundImagem_Latitude");
        }
    }
}
