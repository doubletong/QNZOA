using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIGOA.Infrastructure.Email
{
    public class EmailService : IEmailService
    {

        public void SendMail(string sender, string senderEmail, string mailTo, string mailcc, string subject, string body,
          string smtpServer, string fromEmail, string displayName, string userName, string password, int port, bool enableSsl)
        {
            //var message = new MimeMailMessage();


            //message.To.Add(mailTo);
            //if (!string.IsNullOrEmpty(mailcc))
            //    message.CC.Add(mailcc);

            //message.Subject = subject;
            //message.Body = body; //string.Format("<p>{0}</p><p>发件人：{1} ({2}), 发件人邮箱：{3}</p>", body, name, phone, from);
            //message.IsBodyHtml = true;

            //message.ReplyToList.Add(new MailAddress(senderEmail, sender));
            ////if (!string.IsNullOrEmpty(mailcc))
            ////    message.ReplyToList.Add(new MailAddress(mailTo, sender));

            //message.Sender = new MailAddress(fromEmail, displayName);
            //message.From = new MailAddress(fromEmail, displayName);
            //SmtpClient smtpClient = new SmtpClient(smtpServer, port)
            //{
            //    UseDefaultCredentials = true,
            //    EnableSsl = enableSsl,
            //    //   smtpClient.Port = SettingsManager.SMTP.Port;
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    Credentials = new NetworkCredential(userName, password)
            //};

            //smtpClient.Send(message);

            var message = new MimeMessage();
            message.Sender = new MailboxAddress(displayName, fromEmail);
            message.From.Add(new MailboxAddress(displayName, fromEmail));
            message.To.Add(new MailboxAddress(mailTo));

            if (!string.IsNullOrEmpty(mailcc))
                message.Cc.Add(new MailboxAddress(mailcc));

            message.ReplyTo.Add(new MailboxAddress(sender,senderEmail));

            message.Subject = subject;

            //var plain = new TextPart("plain")
            //{
            //    Text = @"不好意思，我在测试程序，刚才把QQ号写错了，Sorry！"
            //};
            var html = new TextPart("html")
            {
                Text = body
            };
            // create an image attachment for the file located at path
            //var attachment = new MimePart("image", "png")
            //{
            //    ContentObject = new ContentObject(File.OpenRead(path), ContentEncoding.Default),
            //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            //    ContentTransferEncoding = ContentEncoding.Base64,
            //    FileName = Path.GetFileName(path)
            //};

            var alternative = new Multipart("alternative");
            //alternative.Add(plain);
            alternative.Add(html);

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed");
            multipart.Add(alternative);
           // multipart.Add(attachment);
            message.Body = multipart;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpServer, port, enableSsl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(userName, password);

                client.Send(message);
                client.Disconnect(true);
            }
        }


        public void SendMultMail(string sender, string senderEmail, string[] mailTo,  string subject, string body,
          string smtpServer, string fromEmail, string displayName, string userName, string password, int port, bool enableSsl)
        {
            

            var message = new MimeMessage();
            message.Sender = new MailboxAddress(displayName, fromEmail);
            message.From.Add(new MailboxAddress(displayName, fromEmail));
            foreach(var email in mailTo)
            {
                message.To.Add(new MailboxAddress(email));
            }
         
            message.ReplyTo.Add(new MailboxAddress(sender, senderEmail));
            message.Subject = subject;

          
            var html = new TextPart("html")
            {
                Text = body
            };         

            var alternative = new Multipart("alternative");
            //alternative.Add(plain);
            alternative.Add(html);

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed");
            multipart.Add(alternative);
            // multipart.Add(attachment);
            message.Body = multipart;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpServer, port, enableSsl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(userName, password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
