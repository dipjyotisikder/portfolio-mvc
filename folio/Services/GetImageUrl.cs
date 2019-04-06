using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace folio.Services
{
    public class GetImageUrl
    {

        public string Get(HttpPostedFileBase imageFile)
        {
            string filename = Path.GetFileNameWithoutExtension(imageFile.FileName);
            string extension = Path.GetExtension(imageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssff") + extension;
            imageFile.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath("/Content/Admin/img"), filename));
            var imageUrl = "/Content/Admin/img/" + filename;
            return imageUrl;
        }
    }
}