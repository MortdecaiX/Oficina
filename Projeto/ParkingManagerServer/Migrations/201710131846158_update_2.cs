namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsuarioModels", "Sobrenome", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsuarioModels", "Sobrenome");
        }
    }
}
