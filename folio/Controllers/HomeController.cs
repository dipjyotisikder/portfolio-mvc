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
using System.Net.Mail;
using WorkBase.services.concretes;
using folio.Models.Email;
using folio.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace folio.Areas.Portfolio.Controllers
{
    public class HomeController : Controller
    {
        // GET: Portfolio/Home
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var skills = await db.Pskills
                .Include(sk => sk.Pimages)
                .ToListAsync();

            var projects = await db.Projects
                .Include(p => p.ProjectImages)
                .Include(p => p.ProjectSkills)
                .Take(3)
                .ToListAsync();

            var vm = new HomeViewModel
            {
                Pskills = skills,
                Projects = projects,
                ProjectCount = db.Projects.Count()
            };
            return View(vm);
        }

        public async Task<ActionResult> SingleProject(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var single = new HomeViewModel();

            var isingle = db.Projects
                .Include(c => c.ProjectFeatures)
                .Include(c => c.ProjectImages)
                .Include(c => c.ProjectSkills)
                .Where(c => c.Id == id);
            if (isingle != null)
            {
                single.Project = isingle.Single();
                single.Pskills = await db.Pskills.Include(c => c.ProjectSkills).ToListAsync();
                single.ProjectSkills = await db.ProjectSkills.Where(c => c.ProjectId == id).ToListAsync();
                single.ProjectFeatures = await db.ProjectFeatures.Where(c => c.ProjectId == id).ToListAsync();
            }
            return View(single);
        }

        public async Task<ActionResult> ProjectGallery()
        {
            var homevm = new HomeViewModel();
            homevm.Projects = await db.Projects.Include(c => c.ProjectImages)
                .Include(c => c.ProjectSkills)
                .ToListAsync();
            homevm.Pskills = await db.Pskills.ToListAsync();
            return View(homevm);
        }

        public ActionResult Contact(HomeViewModel model)
        {
            return View(model);
        }

        bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(string name, string email, string subject, string message)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                var model = new HomeViewModel
                {
                    Status = new StatusMessageViewModel
                    {
                        Message = "Please fillup all the fields",
                        Type = "error"
                    }
                };
                return View("Contact", model);
            }
            if (!IsValidEmail(email))
            {
                var model = new HomeViewModel
                {
                    Status = new StatusMessageViewModel
                    {
                        Message = "Email address is invalid!",
                        Type = "error"
                    }
                };
                return View("Contact", model);
            }

            //save the informations in database
            var info = new EmailInfo
            {
                Name = name,
                Email = email,
                Subject = subject,
                Message = message
            };
            db.EmailInfos.Add(info);
            await db.SaveChangesAsync();
            try
            {
                var sender = new EmailSender();
                await sender.SendAsync(subject, message, new string[] { "dipjyotisikder@gmail.com" });
                //after successfully email sending complete
                var model = new HomeViewModel
                {
                    Status = new StatusMessageViewModel
                    {
                        Message = "Thanks for your valuable message!",
                        Type = "success"
                    }
                };
                return View("Contact", model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
                var model = new HomeViewModel
                {
                    Status = new StatusMessageViewModel
                    {
                        Message = "Something error occured!",
                        Type = "error"
                    }
                };
                return View("Contact", model);
            }
        }


        public PartialViewResult _Skill()
        {
            List<Pskill> skills = db.Pskills.ToList();

            return PartialView(skills);
        }
    }
}