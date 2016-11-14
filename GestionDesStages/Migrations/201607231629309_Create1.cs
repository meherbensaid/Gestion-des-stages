namespace GestionDesStages.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stages", "Departement_DepartementID", "dbo.Departements");
            DropIndex("dbo.Stages", new[] { "Departement_DepartementID" });
            RenameColumn(table: "dbo.Stages", name: "Departement_DepartementID", newName: "DepartementID");
            AlterColumn("dbo.Stages", "DepartementID", c => c.Int(nullable: false));
            CreateIndex("dbo.Stages", "DepartementID");
            AddForeignKey("dbo.Stages", "DepartementID", "dbo.Departements", "DepartementID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stages", "DepartementID", "dbo.Departements");
            DropIndex("dbo.Stages", new[] { "DepartementID" });
            AlterColumn("dbo.Stages", "DepartementID", c => c.Int());
            RenameColumn(table: "dbo.Stages", name: "DepartementID", newName: "Departement_DepartementID");
            CreateIndex("dbo.Stages", "Departement_DepartementID");
            AddForeignKey("dbo.Stages", "Departement_DepartementID", "dbo.Departements", "DepartementID");
        }
    }
}
