using GestionDesStages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDesStages.ViewModels
{
    public class EncadrantIndexData
    {
        public IEnumerable<Encadrant> Encadrants { get; set; }
        public IEnumerable<Stage> Stages { get; set; }
        public IEnumerable<Stagiaire> Stagiaires { get; set; }
        public int age { get; set; }
    }
}