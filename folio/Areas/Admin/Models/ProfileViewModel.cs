using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace folio.Areas.Admin.Models
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }


        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }


        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }


    public class PicUploadViewModel
    {

        public string ImageUrl { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }



    }
}