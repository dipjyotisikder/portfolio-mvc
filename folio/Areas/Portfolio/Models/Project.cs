using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace folio.Areas.Portfolio.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }



    }


    public class ProjectImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}