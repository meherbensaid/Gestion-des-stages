namespace GestionDesStages.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encadrants", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Encadrants", "Email");
        }
    }
}
