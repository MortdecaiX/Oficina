namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustesEntidades2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstacionamentoModels", "Zoom", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstacionamentoModels", "Zoom");
        }
    }
}
