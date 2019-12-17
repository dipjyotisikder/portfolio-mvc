using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using folio.Models;
using folio.Constants;

namespace WorkBase.services.concretes
{
    public class EmailSender
    {
        //private readonly EmailOptions _emailOptions;


        public async Task SendAsync(string subject, string content, string[] tos = null, string[] ccs = null, string[] bccs = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Constants.EmailOptions.SenderName, Constants.EmailOptions.SenderEmailAddress));

            if (tos != null && tos.Length > 0)
            {
                foreach (var to in tos)
                {
                    message.To.Add(new MailboxAddress("", to));
                }
            }

            if (ccs != null && ccs.Length > 0)
            {
                foreach (var cc in ccs)
                {
                    message.Cc.Add(new MailboxAddress("", cc));
                }
            }

            if (bccs != null && bccs.Length > 0)
            {
                foreach (var bcc in bccs)
                {
                    message.Bcc.Add(new MailboxAddress("", bcc));
                }
            }

            message.Subject = subject;

            var body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = content };

            message.Body = body;

            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential
                {
                    UserName = Constants.EmailOptions.SenderEmailAddress,
                    Password = Constants.EmailOptions.Password
                };

                //client.SslProtocols = System.Security.Authentication.SslProtocols.None;

                await client.ConnectAsync(Constants.EmailOptions.SmtpServer, Convert.ToInt32(Constants.EmailOptions.SmtpPort), SecureSocketOptions.Auto).ConfigureAwait(false);

                try
                {
                    await client.AuthenticateAsync(credentials).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw e;
                }
                try
                {
                    await client.SendAsync(message).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        private Multipart CreateMultipartMessage(MimeEntity body, Dictionary<string, string[]> files)
        {
            var multipart = new Multipart("mixed");

            foreach (string key in files.Keys)
            {
                for (int i = 0; i < files[key].Length; i++)
                {
                    var attachment = new MimePart("image", "jpg")
                    {
                        Content = new MimeContent(new MemoryStream(Convert.FromBase64String(files[key][i])), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = $"{key}-{i}.jpg"
                    };

                    multipart.Add(attachment);
                }
            }
            multipart.Add(body);
            return multipart;
        }
    }
}
