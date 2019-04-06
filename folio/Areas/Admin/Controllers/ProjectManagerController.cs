using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using folio.Areas.Portfolio.Models;
using folio.Models;
using folio.Areas.Admin.Models;
using folio.Services;

namespace folio.Areas.Admin.Controllers
{
    [Authorize]
    public class ProjectManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private List<HttpPostedFileBase> imageFiles = new List<HttpPostedFileBase>();

        // GET: Admin/ProjectManager
        public async Task<ActionResult> Index()
        {

            var ps = new IndexVM
            {
                Projects = db.Projects
       .Include(c => c.ProjectImages)
       .Include(c => c.ProjectSkills).ToList(),
                Pskills = db.Pskills.ToList(),
                ProjectSkills = db.ProjectSkills.ToList(),
                ProjectFeatures = db.ProjectFeatures.ToList()
            };



            return View(ps);
        }


        // GET: Admin/ProjectManager/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Admin/ProjectManager/Create
        public ActionResult Create()


        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectViewModel projectView)
        {
            //check if duplicate entry in multiple image upload
            var count = 0;
            foreach (var item in projectView.ImageFiles)
            {
                if (item != null)
                {
                    imageFiles.Add(item);
                    count++;
                }
            }
            if (ModelState.IsValid)
            {
                for (int i = 0; i < imageFiles.Count; i++)
                {
                    for (int j = 0; j < imageFiles.Count; j++)
                    {
                        if (i != j && imageFiles[i].FileName == imageFiles[j].FileName)
                        {
                            ViewBag.error = "Same image inserted multiple time!";
                            return View(projectView);
                        }
                    }
                }
                //END check if duplicate entry in multiple image upload

                var project = new Project
                {
                    Name = projectView.Name,
                    Description = projectView.Description
                };
                db.Projects.Add(project);
                await db.SaveChangesAsync();

                //image upload start
                foreach (var model in imageFiles)
                {
                    GetImageUrl getImageUrl = new GetImageUrl();
                    string Url = getImageUrl.Get(model);
                    var finalURL = Url;

                    var image = new ProjectImage
                    {
                        ImageUrl = finalURL,
                        ProjectId = project.Id
                    };

                    db.ProjectImages.Add(image);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            return View(projectView);
        }

        //........................................................create.................




        public async Task<ActionResult> RemovePic(int id)
        {

            var current = await db.ProjectImages.FindAsync(id);

            //remove pic from folder too
            var fullPath = Server.MapPath(current.ImageUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            db.ProjectImages.Remove(current);
            await db.SaveChangesAsync();
            var sid = (int)TempData["id"];

            return RedirectToAction("Edit", new { id = sid });
        }
        //........................................................remove pic............................



        [HttpPost]
        public async Task<ActionResult> RemoveSkill(ProjectViewModel model, int id)
        {

            if (model.AddSkillViewModel.ProjectId == null || model.AddSkillViewModel.SkillId == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var dbState = db.ProjectSkills.Where(c => c.ProjectId == model.AddSkillViewModel.ProjectId && c.SkillId == model.AddSkillViewModel.SkillId);
            if (dbState == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            db.ProjectSkills.Remove(dbState.Single());

            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = model.AddSkillViewModel.ProjectId });
        }
        //........................................................remove pic............................




        // GET: Admin/SkillsManager/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tskill = db.Pskills.ToList();

            if (tskill == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            }

            Project projects = await db.Projects
                .Include(c => c.ProjectImages)
                .Include(c => c.ProjectSkills)
                .SingleOrDefaultAsync(c => c.Id == id);


            if (projects == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ps = db.ProjectSkills.Where(c => c.ProjectId == projects.Id).ToList();
            var addvm = new AddSkillViewModel { ProjectId = projects.Id };

            var vm = new ProjectViewModel
            {
                Id = projects.Id,
                Name = projects.Name,
                Description = projects.Description,
                ProjectImages = projects.ProjectImages,
                ProjectSkills = ps,
                TotalSkills = tskill,
                AddSkillViewModel = addvm
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProjectViewModel projectView)
        {
            var project = await db.Projects
                .Include(c => c.ProjectImages)
                .Include(c => c.ProjectSkills)
                .SingleOrDefaultAsync(c => c.Id == projectView.Id);
            if (project == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {

                project.Name = projectView.Name;
                project.Description = projectView.Description;
                db.Entry(project).State = EntityState.Modified;

                await db.SaveChangesAsync();
                if (projectView.ImageFile != null)
                {
                    GetImageUrl getImageUrl = new GetImageUrl();
                    string Url = getImageUrl.Get(projectView.ImageFile);
                    var finalURL = Url;

                    var image = new ProjectImage
                    {
                        ImageUrl = finalURL,
                        ProjectId = project.Id
                    };
                    db.ProjectImages.Add(image);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Edit", new { id = project.Id });
            }
            return View(projectView);
        }

        [HttpPost]
        public async Task<ActionResult> AddSkill(ProjectViewModel model, int id)
        {

            if (model.AddSkillViewModel.ProjectId == null || model.AddSkillViewModel.SkillId == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var dbState = db.ProjectSkills.Where(c => c.ProjectId == model.AddSkillViewModel.ProjectId && c.SkillId == model.AddSkillViewModel.SkillId);
            if (dbState.ToList().Count != 0)
            {
                ModelState.AddModelError("duplicate", "You already have it");
                return RedirectToAction("Edit", new { id = model.AddSkillViewModel.ProjectId });
            }

            var ps = new ProjectSkill
            {
                ProjectId = model.AddSkillViewModel.ProjectId,
                SkillId = model.AddSkillViewModel.SkillId
            };
            db.ProjectSkills.Add(ps);

            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = model.AddSkillViewModel.ProjectId });
        }




        // GET: Admin/ProjectManager/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Admin/ProjectManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Project project = await db.Projects.FindAsync(id);
            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
