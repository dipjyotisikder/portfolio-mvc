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

namespace folio.Areas.Admin.Controllers
{
    [Authorize]
    public class CategoryManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/CategoryManager
        public async Task<ActionResult> Index()
        {
            return View(await db.Pcategories.ToListAsync());
        }

        // GET: Admin/CategoryManager/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pcategory pcategory = await db.Pcategories.FindAsync(id);
            if (pcategory == null)
            {
                return HttpNotFound();
            }
            return View(pcategory);
        }

        // GET: Admin/CategoryManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/CategoryManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Pcategory pcategory)
        {
            if (ModelState.IsValid)
            {
                db.Pcategories.Add(pcategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pcategory);
        }

        // GET: Admin/CategoryManager/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pcategory pcategory = await db.Pcategories.FindAsync(id);
            if (pcategory == null)
            {
                return HttpNotFound();
            }
            return View(pcategory);
        }

        // POST: Admin/CategoryManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Pcategory pcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pcategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pcategory);
        }

        // GET: Admin/CategoryManager/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pcategory pcategory = await db.Pcategories.FindAsync(id);
            if (pcategory == null)
            {
                return HttpNotFound();
            }
            return View(pcategory);
        }

        // POST: Admin/CategoryManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pcategory pcategory = await db.Pcategories.FindAsync(id);
            db.Pcategories.Remove(pcategory);
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
