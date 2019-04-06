using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace folio.Areas.Portfolio.Models
{
    public class Pimage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }


        public int SkillId { get; set; }

        [ForeignKey("SkillId")]
        public Pskill Skill { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
