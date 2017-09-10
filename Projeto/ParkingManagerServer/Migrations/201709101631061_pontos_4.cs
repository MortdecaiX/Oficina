namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pontos_4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstacionamentoModels", "ImagemAltura", c => c.Long(nullable: false));
            AddColumn("dbo.EstacionamentoModels", "ImagemLargura", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstacionamentoModels", "ImagemLargura");
            DropColumn("dbo.EstacionamentoModels", "ImagemAltura");
        }
    }
}
