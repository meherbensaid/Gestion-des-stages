using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionDesStages.Models
{
    public class Stagiaire
    {
        public int StagiaireID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "le nom ne doit pas depasser 50 caractéres")]
        public String Nom { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "le prenom ne doit pas depasser 50 caractéres")]
        public String Prenom { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date de naissance")]
        public DateTime DateDeNaissance { get; set; }
        [Required]
        [Display(Name = "Lieu de naissance")]
        public String LieuDeNaissance { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Courrier électronique")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "etes-vous etudiants ? ")]
        public bool Student { get; set; }


        public virtual ICollection<Stage> Stages { get; set; }



        public List<String> Search(String Nom)
        {
            ApplicationDbContext Context = new ApplicationDbContext();

            List<String> NomsStagiaires = Context.Stagiaires.Where(s => s.Nom.StartsWith(Nom))
                .Select(s=>s.Nom).ToList();
            return NomsStagiaires;

        }
    }

}