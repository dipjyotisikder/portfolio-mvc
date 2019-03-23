using folio.Areas.Portfolio.Models;
using folio.Areas.Portfolio.Models.ViewModels;
using folio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace folio.Areas.Portfolio.Controllers
{
    public class HomeController : Controller
    {
        // GET: Portfolio/Home
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var skills = db.Pskills.Include(sk => sk.Category).Include(sk => sk.Pimages).ToList();
            var vm = new HomeViewModel
            {
                Pskills = skills
            };

            return View(vm);
        }


        public PartialViewResult _Skill()
        {
            List<Pskill> skills = db.Pskills.ToList();

            return PartialView(skills);
        }
    }
}