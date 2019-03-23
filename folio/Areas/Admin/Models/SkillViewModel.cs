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


        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Skill Category")]
        public int CategoryId { get; set; }

        [NotMapped]
        public List<HttpPostedFileBase> ImageFiles { get; set; }
        [MaxLength(2)]
        [DisplayName("Strength(Out of 10)")]
        public int Strength { get; set; }
    }


    public static class ImageHelper
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string height)

        {

            var builder = new TagBuilder("img");

            builder.MergeAttribute("src", src);

            builder.MergeAttribute("alt", altText);

            builder.MergeAttribute("height", height);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));

        }

    }
}