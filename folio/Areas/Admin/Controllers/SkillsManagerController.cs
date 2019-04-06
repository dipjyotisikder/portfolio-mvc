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
using System.IO;
using folio.Services;

namespace folio.Areas.Admin.Controllers
{
    [Authorize]
    public class SkillsManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<HttpPostedFileBase> imageFiles = new List<HttpPostedFileBase>();


        // GET: Admin/SkillsManager
        public async Task<ActionResult> Index()
        {

            var pskills = db.Pskills
                .Include(c => c.Pimages);
            var Pskills = await pskills.ToListAsync();
            return View(Pskills);
        }

        // GET: Admin/SkillsManager/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pskill pskill = await db.Pskills.FindAsync(id);
            if (pskill == null)
            {
                return HttpNotFound();
            }
            return View(pskill);
        }

        // GET: Admin/SkillsManager/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Pcategories, "Id", "Name");
            return View();
        }

        // POST: Admin/SkillsManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SkillViewModel skillView)
        {

            //check if duplicate entry in multiple image upload
            var count = 0;
            foreach (var item in skillView.ImageFiles)
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
                            ViewBag.CategoryId = new SelectList(db.Pcategories, "Id", "Name", skillView.CategoryId);
                            ViewBag.error = "Same image inserted multiple time!";
                            return View(skillView);
                        }
                    }
                }
                //END check if duplicate entry in multiple image upload

                var skill = new Pskill
                {
                    Name = skillView.Name,
                    Description = skillView.Description,
                    Strength = skillView.Strength,
                    CategoryId = skillView.CategoryId
                };
                db.Pskills.Add(skill);
                await db.SaveChangesAsync();

                //image upload start
                foreach (var model in imageFiles)
                {

                    GetImageUrl getImageUrl = new GetImageUrl();
                    string Url = getImageUrl.Get(model);
                    var finalURL = Url;

                    var image = new Pimage
                    {
                        ImageUrl = finalURL,
                        SkillId = skill.Id
                    };

                    db.Pimages.Add(image);
                    await db.SaveChangesAsync();

                }
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Pcategories, "Id", "Name", skillView.CategoryId);
            return View(skillView);
        }



        // GET: Admin/SkillsManager/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pskill pskill = await db.Pskills
                .Include(c => c.Pimages)
                .Include(c => c.ProjectSkills)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (pskill == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var vm = new SkillViewModel
            {
                Id = pskill.Id,
                Name = pskill.Name,
                Description = pskill.Description,
                Strength = pskill.Strength,
                Pimages = pskill.Pimages,
                CategoryId = pskill.CategoryId,

            };
            ViewBag.CategoryId = new SelectList(db.Pcategories, "Id", "Name", pskill.CategoryId);
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SkillViewModel skill)
        {
            var dbSkill = await db.Pskills
                .Include(c => c.Pimages)
                .Include(c => c.ProjectSkills)
                .SingleOrDefaultAsync(c => c.Id == skill.Id);

            if (dbSkill == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {

                dbSkill.Name = skill.Name;
                dbSkill.Description = skill.Description;
                dbSkill.CategoryId = skill.CategoryId;
                dbSkill.Strength = skill.Strength;

                db.Entry(dbSkill).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (skill.ImageFile != null)
                {

                    GetImageUrl getImageUrl = new GetImageUrl();
                    string Url = getImageUrl.Get(skill.ImageFile);
                    var finalURL = Url;

                    var image = new Pimage
                    {
                        ImageUrl = finalURL,
                        SkillId = dbSkill.Id
                    };

                    db.Pimages.Add(image);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index");

            }
            ViewBag.CategoryId = new SelectList(db.Pcategories, "Id", "Name", dbSkill.CategoryId);
            return View(dbSkill);
        }

        public async Task<ActionResult> RemovePic(int id)
        {

            var current = await db.Pimages.FindAsync(id);

            //remove pic from folder too
            var fullPath = Server.MapPath(current.ImageUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            db.Pimages.Remove(current);
            await db.SaveChangesAsync();
            var sid = (int)TempData["id"];

            return RedirectToAction("Edit", new { id = sid });
        }


        // GET: Admin/SkillsManager/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pskill pskill = await db.Pskills.FindAsync(id);
            if (pskill == null)
            {
                return HttpNotFound();
            }
            return View(pskill);
        }

        // POST: Admin/SkillsManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pskill pskill = await db.Pskills.FindAsync(id);
            db.Pskills.Remove(pskill);
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
