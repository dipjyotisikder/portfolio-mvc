using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace folio.Areas.Portfolio.Models
{
    public class Pskill
    {

        public Pskill()
        {
            Pimages = new List<Pimage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Strength { get; set; }


        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]

        public virtual Pcategory Category { get; set; }

        public virtual List<Pimage> Pimages { get; set; }

    }
}