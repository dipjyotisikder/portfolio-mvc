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
        private HttpServerUtilityBase utilityBase;
        private List<HttpPostedFileBase> imageFiles = new List<HttpPostedFileBase>();

        public SkillsManagerController(HttpServerUtilityBase utility)
        {
            utilityBase = utility;
        }

        // GET: Admin/SkillsManager
        public async Task<ActionResult> Index()
        {

            var pskills = db.Pskills
                .Include(c => c.Pimages)
                .Include(p => p.Category);


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
                    //string filename = Path.GetFileNameWithoutExtension(model.FileName);
                    //string extension = Path.GetExtension(model.FileName);
                    //filename = filename + DateTime.Now.ToString("yymmssff") + extension;
                    //model.SaveAs(Path.Combine(Server.MapPath("/Content/Admin/img"), filename));

                    //function for getting the image
                    GetImageUrl getImageUrl = new GetImageUrl();
                    string Url = getImageUrl.Get(model, utilityBase);
                    var finalURL = Url;

                    var image = new Pimage
                    {
                        ImageUrl = finalURL,
                        SkillId = skill.Id
                    };

                    db.Pimages.Add(image);
                    await db.SaveChangesAsync();

                }
                return Json(JsonRequestBehavior.AllowGet);
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
            Pskill pskill = await db.Pskills.FindAsync(id);
            if (pskill == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Pcategories, "Id", "Name", pskill.CategoryId);
            return View(pskill);
        }

        // POST: Admin/SkillsManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,CategoryId")] Pskill pskill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pskill).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Pcategories, "Id", "Name", pskill.CategoryId);
            return View(pskill);
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
