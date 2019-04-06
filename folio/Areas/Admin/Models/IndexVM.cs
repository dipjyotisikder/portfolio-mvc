using folio.Areas.Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace folio.Areas.Admin.Models
{
    public class IndexVM
    {
        public List<Project> Projects { get; set; }
        public List<Pskill> Pskills { get; set; }
        public List<ProjectSkill> ProjectSkills { get; set; }
        public List<ProjectFeature> ProjectFeatures { get; set; }
    }
}