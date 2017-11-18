namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3423k4jksdjfasdf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsuarioModels", "DataNascimento", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsuarioModels", "DataNascimento");
        }
    }
}
