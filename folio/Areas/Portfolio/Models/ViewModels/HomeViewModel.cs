using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace folio.Areas.Portfolio.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Pskill> Pskills { get; set; }
        public Pskill Pskill { get; set; }

        public List<Project> Projects { get; set; }
        public Project Project { get; set; }

        public List<ProjectSkill> ProjectSkills { get; set; }
        public List<ProjectFeature> ProjectFeatures { get; set; }


    }
}