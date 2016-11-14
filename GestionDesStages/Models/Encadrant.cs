using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionDesStages.Models
{
    
    public class Encadrant
    {
        public int EncadrantID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "le nom ne doit pas depasser 50 caractéres")]
        public String Nom { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "le prenom ne doit pas depasser 50 caractéres")]
        public String Prenom { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [Display(Name = "date d'embauche")]
        public DateTime DateEmbauche { get; set; }

   
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
      
        public string Password { get; set; }

        [Required]
        [Display(Name = "Courrier électronique")]
        [EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<Stage> Stages { get; set; }
        public virtual Bureau  Bureau { get; set; }
        
        public virtual Departement Departement { get; set; }

    }
}