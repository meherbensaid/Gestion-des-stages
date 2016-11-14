using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDesStages.ViewModels
{
    public class AssignedStageData
    {
        public int StageID { get; set; }
        public String Titre { get; set; }
        public bool Assigned { get; set; }
    }
}