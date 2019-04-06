using folio.Areas.Portfolio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace folio.Areas.Admin.Models
{
    public class SkillViewModel
    {
        public int Id { get; set; }


        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Skill Category")]
        public int CategoryId { get; set; }

        [NotMapped]
        public List<HttpPostedFileBase> ImageFiles { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }


        public virtual List<Pimage> Pimages { get; set; }

        public List<Project> Projects { get; set; }



        [DisplayName("Strength(Out of 10)")]
        public int Strength { get; set; }
    }



}