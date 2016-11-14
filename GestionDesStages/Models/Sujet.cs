using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestionDesStages.Models
{
    public class Sujet
    {
        [Key]
        [ForeignKey("Stage")]
        public int StageID { get; set; }

        public String Titre { get; set; }
        public String Description { get; set; }
        public Stage Stage { get; set; }
    }
}