using folio.Areas.Portfolio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace folio.Areas.Admin.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }


        [NotMapped]
        public List<HttpPostedFileBase> ImageFiles { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public List<ProjectImage> ProjectImages { get; set; }
        public List<ProjectSkill> ProjectSkills { get; set; }

        public AddSkillViewModel AddSkillViewModel { get; set; }

        public Pskill Pskill { get; set; }
        public IEnumerable<Pskill> TotalSkills { get; set; }

    }
}