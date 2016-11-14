using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesStages.Models
{
    public class Bureau
    {
        [Key]
        [ForeignKey("Encadrant")]
        public int EncadrantID { get; set; }
        [StringLength(50)]
        [Display(Name = "Localisation")]
        public string Localisation { get; set; }
        public virtual Encadrant Encadrant { get; set; }
    }
}