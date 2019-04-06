using folio.Areas.Portfolio.Models;
using folio.Areas.Portfolio.Models.ViewModels;
using folio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net;

namespace folio.Areas.Portfolio.Controllers
{
    public class HomeController : Controller
    {
        // GET: Portfolio/Home
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var skills = await db.Pskills.Include(sk => sk.Pimages).ToListAsync();
            var projects = await db.Projects.Include(p => p.ProjectImages).Include(p => p.ProjectSkills).ToListAsync();
            var vm = new HomeViewModel
            {
                Pskills = skills,
                Projects = projects
            };
            return View(vm);
        }

        public async Task<ActionResult> SingleProject(int id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var single = new HomeViewModel();

            var isingle = db.Projects.Include(c => c.ProjectImages).Include(c => c.ProjectSkills).Where(c => c.Id == id);
            if (isingle != null)
            {
                single.Project = isingle.Single();
                single.Pskills = await db.Pskills.Include(c => c.ProjectSkills).ToListAsync();
                single.ProjectSkills = await db.ProjectSkills.Where(c => c.ProjectId == id).ToListAsync();
            }
            return View(single);
        }



        public PartialViewResult _Skill()
        {
            List<Pskill> skills = db.Pskills.ToList();

            return PartialView(skills);
        }
    }
}