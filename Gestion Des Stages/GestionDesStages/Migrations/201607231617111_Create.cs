namespace GestionDesStages.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bureaux",
                c => new
                    {
                        EncadrantID = c.Int(nullable: false),
                        Localisation = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.EncadrantID)
                .ForeignKey("dbo.Encadrants", t => t.EncadrantID)
                .Index(t => t.EncadrantID);
            
            CreateTable(
                "dbo.Encadrants",
                c => new
                    {
                        EncadrantID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 50),
                        Prenom = c.String(nullable: false, maxLength: 50),
                        DateEmbauche = c.DateTime(nullable: false),
                        Departement_DepartementID = c.Int(),
                    })
                .PrimaryKey(t => t.EncadrantID)
                .ForeignKey("dbo.Departements", t => t.Departement_DepartementID)
                .Index(t => t.Departement_DepartementID);
            
            CreateTable(
                "dbo.Departements",
                c => new
                    {
                        DepartementID = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DepartementID);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        StageID = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 50),
                        DateDebut = c.DateTime(nullable: false),
                        DateFin = c.DateTime(nullable: false),
                        Type = c.String(),
                        valide = c.Boolean(nullable: false),
                        Departement_DepartementID = c.Int(),
                    })
                .PrimaryKey(t => t.StageID)
                .ForeignKey("dbo.Departements", t => t.Departement_DepartementID)
                .Index(t => t.Departement_DepartementID);
            
            CreateTable(
                "dbo.Stagiaires",
                c => new
                    {
                        StagiaireID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 50),
                        Prenom = c.String(nullable: false, maxLength: 50),
                        Age = c.Int(nullable: false),
                        DateDeNaissance = c.DateTime(nullable: false),
                        LieuDeNaissance = c.String(nullable: false),
                        Student = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StagiaireID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Sujets",
                c => new
                    {
                        StageID = c.Int(nullable: false),
                        Titre = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.StageID)
                .ForeignKey("dbo.Stages", t => t.StageID)
                .Index(t => t.StageID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.StageEncadrants",
                c => new
                    {
                        Stage_StageID = c.Int(nullable: false),
                        Encadrant_EncadrantID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stage_StageID, t.Encadrant_EncadrantID })
                .ForeignKey("dbo.Stages", t => t.Stage_StageID, cascadeDelete: true)
                .ForeignKey("dbo.Encadrants", t => t.Encadrant_EncadrantID, cascadeDelete: true)
                .Index(t => t.Stage_StageID)
                .Index(t => t.Encadrant_EncadrantID);
            
            CreateTable(
                "dbo.StagiaireStages",
                c => new
                    {
                        Stagiaire_StagiaireID = c.Int(nullable: false),
                        Stage_StageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stagiaire_StagiaireID, t.Stage_StageID })
                .ForeignKey("dbo.Stagiaires", t => t.Stagiaire_StagiaireID, cascadeDelete: true)
                .ForeignKey("dbo.Stages", t => t.Stage_StageID, cascadeDelete: true)
                .Index(t => t.Stagiaire_StagiaireID)
                .Index(t => t.Stage_StageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Sujets", "StageID", "dbo.Stages");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Bureaux", "EncadrantID", "dbo.Encadrants");
            DropForeignKey("dbo.StagiaireStages", "Stage_StageID", "dbo.Stages");
            DropForeignKey("dbo.StagiaireStages", "Stagiaire_StagiaireID", "dbo.Stagiaires");
            DropForeignKey("dbo.StageEncadrants", "Encadrant_EncadrantID", "dbo.Encadrants");
            DropForeignKey("dbo.StageEncadrants", "Stage_StageID", "dbo.Stages");
            DropForeignKey("dbo.Stages", "Departement_DepartementID", "dbo.Departements");
            DropForeignKey("dbo.Encadrants", "Departement_DepartementID", "dbo.Departements");
            DropIndex("dbo.StagiaireStages", new[] { "Stage_StageID" });
            DropIndex("dbo.StagiaireStages", new[] { "Stagiaire_StagiaireID" });
            DropIndex("dbo.StageEncadrants", new[] { "Encadrant_EncadrantID" });
            DropIndex("dbo.StageEncadrants", new[] { "Stage_StageID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Sujets", new[] { "StageID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Stages", new[] { "Departement_DepartementID" });
            DropIndex("dbo.Encadrants", new[] { "Departement_DepartementID" });
            DropIndex("dbo.Bureaux", new[] { "EncadrantID" });
            DropTable("dbo.StagiaireStages");
            DropTable("dbo.StageEncadrants");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Sujets");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Stagiaires");
            DropTable("dbo.Stages");
            DropTable("dbo.Departements");
            DropTable("dbo.Encadrants");
            DropTable("dbo.Bureaux");
        }
    }
}
