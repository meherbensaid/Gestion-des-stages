namespace GestionDesStages.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StageAttributeDisponible : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stages", "Disponible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stages", "Disponible");
        }
    }
}
