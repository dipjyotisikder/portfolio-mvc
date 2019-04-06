using folio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace folio.Areas.Portfolio.Models
{
    public class ProjectSkill
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private List<Pskill> pskills;

        [Key, Column(Order = 0)]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [Key, Column(Order = 1)]
        public int SkillId { get; set; }

        [ForeignKey("SkillId")]
        public Pskill Pskill { get; set; }



        public List<Pskill> GetSkills()
        {
            var ids = context.ProjectSkills.Where(c => c.ProjectId == ProjectId).Select(c => c.SkillId).ToList();

            foreach (var item in ids)
            {
                var skill = context.Pskills.Find(item);
                pskills.Add(skill);
            }
            return pskills;
        }
    }

}