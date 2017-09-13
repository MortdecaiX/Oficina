namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ponto5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstacionamentoModels", "ImagemRotacao", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstacionamentoModels", "ImagemRotacao");
        }
    }
}
