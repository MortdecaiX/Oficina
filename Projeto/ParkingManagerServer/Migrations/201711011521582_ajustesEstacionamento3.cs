namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustesEstacionamento3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsuarioModels", "VagaEspecial", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsuarioModels", "VagaEspecial");
        }
    }
}
