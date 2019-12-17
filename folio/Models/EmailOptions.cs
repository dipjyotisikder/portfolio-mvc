using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace folio.Models
{
    public class EmailOptions
    {
        public bool Enabled { get; set; }
        public string SenderName { get; set; }
        public string SenderEmailAddress { get; set; }
        public string Password { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public string Tls { get; set; }
        public long Timeout { get; set; }
    }
}