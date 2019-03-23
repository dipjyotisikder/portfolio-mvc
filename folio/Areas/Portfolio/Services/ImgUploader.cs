using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ProHallManagement.Helpers
{
    public class ImgUploader
    {


        public string imgString;
        private readonly HostingEnvironment _env;

        public ImgUploader(HostingEnvironment environment)
        {
            _env = environment;
        }

        public ImgUploader()
        {
        }

        public string ImageUrl(HttpPostedFileBase file)
        {


            if (file == null) return null;

            string path_Root = Path.GetFileName(file.FileName);

            string path_to_Images = path_Root + "\\Assets\\images\\products\\" + file.FileName;

            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {

                file.SaveAs(stream.Name);
                string revUrl = Reverse.reverse(path_to_Images);
                int count = 0;
                int flag = 0;

                for (int i = 0; i < revUrl.Length; i++)
                {
                    if (revUrl[i] == '\\')
                    {
                        count++;

                    }
                    if (count == 4)
                    {
                        flag = i;
                        break;
                    }
                }

                string sub = revUrl.Substring(0, flag + 1);
                string finalString = Reverse.reverse(sub);

                string f = finalString.Replace("\\", "/");
                imgString = f;
            }

            return imgString;


        }

    }
}