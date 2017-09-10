namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pontos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstacionamentoModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Nome = c.String(),
                        Imagem = c.String(),
                        LocalizacaoImagem_Latitude = c.Double(nullable: false),
                        LocalizacaoImagem_Longitude = c.Double(nullable: false),
                        LocalizacaoImagem_Altitude = c.Double(nullable: false),
                        Localizacao_Latitude = c.Double(nullable: false),
                        Localizacao_Longitude = c.Double(nullable: false),
                        Localizacao_Altitude = c.Double(nullable: false),
                        Responsavel_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsuarioModels", t => t.Responsavel_Id)
                .Index(t => t.Responsavel_Id);
            
            CreateTable(
                "dbo.PontoModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Localizacao_Latitude = c.Double(nullable: false),
                        Localizacao_Longitude = c.Double(nullable: false),
                        Localizacao_Altitude = c.Double(nullable: false),
                        Entrada = c.Boolean(nullable: false),
                        Saida = c.Boolean(nullable: false),
                        PontoModel_Id = c.Long(nullable: false),
                        EstacionamentoModel_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PontoModels", t => t.PontoModel_Id)
                .ForeignKey("dbo.EstacionamentoModels", t => t.EstacionamentoModel_Id, cascadeDelete: true)
                .Index(t => t.PontoModel_Id)
                .Index(t => t.EstacionamentoModel_Id);
            
            AddColumn("dbo.VagaModels", "PontoModel_Id", c => c.Long());
            CreateIndex("dbo.VagaModels", "PontoModel_Id");
            AddForeignKey("dbo.VagaModels", "PontoModel_Id", "dbo.PontoModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstacionamentoModels", "Responsavel_Id", "dbo.UsuarioModels");
            DropForeignKey("dbo.PontoModels", "EstacionamentoModel_Id", "dbo.EstacionamentoModels");
            DropForeignKey("dbo.VagaModels", "PontoModel_Id", "dbo.PontoModels");
            DropForeignKey("dbo.PontoModels", "PontoModel_Id", "dbo.PontoModels");
            DropIndex("dbo.VagaModels", new[] { "PontoModel_Id" });
            DropIndex("dbo.PontoModels", new[] { "EstacionamentoModel_Id" });
            DropIndex("dbo.PontoModels", new[] { "PontoModel_Id" });
            DropIndex("dbo.EstacionamentoModels", new[] { "Responsavel_Id" });
            DropColumn("dbo.VagaModels", "PontoModel_Id");
            DropTable("dbo.PontoModels");
            DropTable("dbo.EstacionamentoModels");
        }
    }
}
