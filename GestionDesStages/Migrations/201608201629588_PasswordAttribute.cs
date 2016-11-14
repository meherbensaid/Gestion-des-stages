namespace GestionDesStages.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encadrants", "Password", c => c.String(nullable: false));
            AddColumn("dbo.Stagiaires", "Password", c => c.String(nullable: false));
            AddColumn("dbo.Stagiaires", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stagiaires", "Email");
            DropColumn("dbo.Stagiaires", "Password");
            DropColumn("dbo.Encadrants", "Password");
        }
    }
}
