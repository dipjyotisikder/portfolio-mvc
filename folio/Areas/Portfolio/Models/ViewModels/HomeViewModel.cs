using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace folio.Areas.Portfolio.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Pskill> Pskills { get; set; }

        public List<Project> Projects { get; set; }
    }
}