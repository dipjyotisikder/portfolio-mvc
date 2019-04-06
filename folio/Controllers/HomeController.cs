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


        public ActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SendEmail(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("jamilmoughal786@gmail.com", "Jamil");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "Your Email Password here";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View("Contact");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View("Contact");
        }


        public PartialViewResult _Skill()
        {
            List<Pskill> skills = db.Pskills.ToList();

            return PartialView(skills);
        }
    }
}