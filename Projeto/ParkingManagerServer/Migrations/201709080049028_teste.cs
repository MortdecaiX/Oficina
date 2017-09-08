namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teste : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OcupacaoModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DataEntrada = c.DateTime(nullable: false),
                        DataSaida = c.DateTime(nullable: false),
                        Usuario_Id = c.Long(),
                        Veiculo_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsuarioModels", t => t.Usuario_Id)
                .ForeignKey("dbo.VeiculoModels", t => t.Veiculo_Id)
                .Index(t => t.Usuario_Id)
                .Index(t => t.Veiculo_Id);
            
            CreateTable(
                "dbo.UsuarioModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Nome = c.String(),
                        Email = c.String(),
                        Senha = c.String(),
                        CPF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VeiculoModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Placa = c.String(),
                        Marca = c.String(),
                        Modelo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReservaModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        DataEntrada = c.DateTime(nullable: false),
                        DataExpiracao = c.DateTime(nullable: false),
                        DataSaida = c.DateTime(nullable: false),
                        Usuario_Id = c.Long(),
                        Veiculo_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsuarioModels", t => t.Usuario_Id)
                .ForeignKey("dbo.VeiculoModels", t => t.Veiculo_Id)
                .Index(t => t.Usuario_Id)
                .Index(t => t.Veiculo_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.VagaModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Numero = c.Long(nullable: false),
                        Tipo = c.Int(nullable: false),
                        Localizacao_Latitude = c.Double(nullable: false),
                        Localizacao_Longitude = c.Double(nullable: false),
                        Localizacao_Altitude = c.Double(nullable: false),
                        Pavimento = c.Int(nullable: false),
                        Ocupacao_Id = c.Long(),
                        Reserva_Id = c.Long(),
                        Responsavel_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OcupacaoModels", t => t.Ocupacao_Id)
                .ForeignKey("dbo.ReservaModels", t => t.Reserva_Id)
                .ForeignKey("dbo.UsuarioModels", t => t.Responsavel_Id)
                .Index(t => t.Ocupacao_Id)
                .Index(t => t.Reserva_Id)
                .Index(t => t.Responsavel_Id);
            
            CreateTable(
                "dbo.UsuarioVeiculo",
                c => new
                    {
                        UsuarioRefId = c.Long(nullable: false),
                        VeiculoRefId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioRefId, t.VeiculoRefId })
                .ForeignKey("dbo.UsuarioModels", t => t.UsuarioRefId, cascadeDelete: true)
                .ForeignKey("dbo.VeiculoModels", t => t.VeiculoRefId, cascadeDelete: true)
                .Index(t => t.UsuarioRefId)
                .Index(t => t.VeiculoRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VagaModels", "Responsavel_Id", "dbo.UsuarioModels");
            DropForeignKey("dbo.VagaModels", "Reserva_Id", "dbo.ReservaModels");
            DropForeignKey("dbo.VagaModels", "Ocupacao_Id", "dbo.OcupacaoModels");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ReservaModels", "Veiculo_Id", "dbo.VeiculoModels");
            DropForeignKey("dbo.ReservaModels", "Usuario_Id", "dbo.UsuarioModels");
            DropForeignKey("dbo.OcupacaoModels", "Veiculo_Id", "dbo.VeiculoModels");
            DropForeignKey("dbo.OcupacaoModels", "Usuario_Id", "dbo.UsuarioModels");
            DropForeignKey("dbo.UsuarioVeiculo", "VeiculoRefId", "dbo.VeiculoModels");
            DropForeignKey("dbo.UsuarioVeiculo", "UsuarioRefId", "dbo.UsuarioModels");
            DropIndex("dbo.UsuarioVeiculo", new[] { "VeiculoRefId" });
            DropIndex("dbo.UsuarioVeiculo", new[] { "UsuarioRefId" });
            DropIndex("dbo.VagaModels", new[] { "Responsavel_Id" });
            DropIndex("dbo.VagaModels", new[] { "Reserva_Id" });
            DropIndex("dbo.VagaModels", new[] { "Ocupacao_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ReservaModels", new[] { "Veiculo_Id" });
            DropIndex("dbo.ReservaModels", new[] { "Usuario_Id" });
            DropIndex("dbo.OcupacaoModels", new[] { "Veiculo_Id" });
            DropIndex("dbo.OcupacaoModels", new[] { "Usuario_Id" });
            DropTable("dbo.UsuarioVeiculo");
            DropTable("dbo.VagaModels");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ReservaModels");
            DropTable("dbo.VeiculoModels");
            DropTable("dbo.UsuarioModels");
            DropTable("dbo.OcupacaoModels");
        }
    }
}
