using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace folio.Models.Email
{
    public class EmailInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}