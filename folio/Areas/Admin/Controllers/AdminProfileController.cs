using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using folio.Models;
using Microsoft.AspNet.Identity.Owin;
using folio.Areas.Admin.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.IO;
using folio.Services;

namespace folio.Areas.Admin.Controllers
{

    [Authorize]
    public class AdminProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        HttpServerUtilityBase utilityBase;

        private ApplicationUserManager _userManager;

        public AdminProfileController(ApplicationUserManager manager, ApplicationDbContext context, HttpServerUtilityBase httpServerUtility)
        {
            UserManager = manager;
            db = context;
            utilityBase = httpServerUtility;
        }

        public AdminProfileController()
        {

        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Admin/AdminProfile
        public async Task<ActionResult> Index()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);


            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                CurrentAddress = user.CurrentAddress,
                PermanentAddress = user.PermanentAddress
            };

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);

            var usert = await manager.FindByIdAsync(User.Identity.GetUserId());
            ViewBag.img = usert.ImageUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = new ApplicationUser
                //{
                //    FirstName = model.FirstName,
                //    LastName = model.LastName,
                //    PhoneNumber = model.Phone,
                //    CurrentAddress = model.CurrentAddress,
                //    PermanentAddress = model.PermanentAddress,
                //    UserName = User.Identity.Name
                //};

                //var currentUser = await UserManager.FindByNameAsync(User.Identity.Name);
                //currentUser = user;
                //var result = await UserManager.UpdateAsync(currentUser);

                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var manager = new UserManager<ApplicationUser>(store);

                var updatedUser = await manager.FindByIdAsync(User.Identity.GetUserId());

                updatedUser.FirstName = model.FirstName;
                updatedUser.LastName = model.LastName;
                updatedUser.CurrentAddress = model.CurrentAddress;
                updatedUser.PhoneNumber = model.Phone;
                updatedUser.PermanentAddress = model.PermanentAddress;

                await manager.UpdateAsync(updatedUser);

                return RedirectToAction("Index");

            }

            // If we got this far, something failed, redisplay form
            return View("Index", model);
        }



        //profile Picture section goes here

        public async Task<ActionResult> ProfilePicture()
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);

            var usert = await manager.FindByIdAsync(User.Identity.GetUserId());
            ViewBag.img = usert.ImageUrl;
            ViewBag.id = usert.Id;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadPic(PicUploadViewModel model)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);
            var current = await manager.FindByIdAsync(User.Identity.GetUserId());

            if (!string.IsNullOrEmpty(current.ImageUrl))
            {
                var result = "Image Already exists";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (model.ImageFile != null)
            {

                GetImageUrl getImageUrl = new GetImageUrl();
                string Url = getImageUrl.Get(model.ImageFile);


                //string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                //string extension = Path.GetExtension(model.ImageFile.FileName);
                //filename = filename + DateTime.Now.ToString("yymmssff") + extension;
                //model.ImageUrl = filename;
                //model.ImageFile.SaveAs(Path.Combine(Server.MapPath("/Content/Admin/img"), filename));
                //current.ImageUrl = "/Content/Admin/img/" + model.ImageUrl;

                current.ImageUrl = Url;
                await manager.UpdateAsync(current);
                var result = "image uploaded successfully";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("ProfilePicture");
        }

        public async Task<ActionResult> RemovePic()
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);
            var current = await manager.FindByIdAsync(User.Identity.GetUserId());

            //remove pic from folder too
            var fullPath = Server.MapPath(current.ImageUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            current.ImageUrl = null;
            await manager.UpdateAsync(current);
            return RedirectToAction("ProfilePicture");
        }



        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account", new { area = "" });
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
