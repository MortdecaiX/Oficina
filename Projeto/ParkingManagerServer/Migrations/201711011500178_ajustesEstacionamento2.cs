namespace ParkingManagerServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustesEstacionamento2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstacionamentoModels", "ImagemURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstacionamentoModels", "ImagemURL");
        }
    }
}
