using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionDesStages.Models
{
    public class Stage
    {
        public int StageID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Nom { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Data de début du stage")]
        public DateTime DateDebut { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Date de fin du stage")]
        public DateTime DateFin { get; set; }
        //Type du stage : stage d'été , stage PFE ;. ///
        public String  Type { get; set; }
        public Boolean Disponible { get; set; }
        public Boolean valide { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public int DepartementID { get; set; }
        public virtual Departement Departement { get; set; }
        public virtual ICollection<Encadrant> Encadrants { get; set; }
        public virtual ICollection<Stagiaire>  Stagiaire { get; set; }
        public Sujet Sujet { get; set; }



    }


   
}