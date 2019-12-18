using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace folio.Constants
{
    public static class Constants
    {
        public static class EmailOptions
        {
            public static bool Enabled = true;
            public static string SenderName = "Dipjyotis contact us";
            public static string SenderEmailAddress = "ultimacoder@gmail.com";
            public static string Password = "Ultimate@coder";
            public static string SmtpServer = "smtp.gmail.com";
            public static int SmtpPort = 25;
            public static string Tls = "";
            public static int Timeout = 3000;
        }
    }
}