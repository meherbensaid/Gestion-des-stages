using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace GestionDesStages.Models
{
    public class StageContext:DbContext
    {

        public StageContext():base("Stage")
        {

        }

        public DbSet<Stage> Stages { get; set; }
        public DbSet<Stagiaire> Stagiaires { get; set; }
        public DbSet<Encadrant> Encadrants { get; set; }
        public DbSet<Bureau> Bureaux { get; set; }
        public DbSet<Sujet> Sujets { get; set; }
        public DbSet<Departement> Departements { get; set; }

    }
}