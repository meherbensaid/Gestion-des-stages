using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestionDesStages.Models
{
    public class Departement
    {
       
            public int DepartementID { get; set; }
            [StringLength(50, MinimumLength = 3)]
            public string Nom { get; set; }
            public virtual ICollection<Encadrant> Encadrants { get; set; }
            public virtual ICollection<Stage>  Stages { get; set; }

    }
}