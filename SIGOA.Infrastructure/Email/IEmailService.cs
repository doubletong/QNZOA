using System;
using System.Collections.Generic;
using System.Text;

namespace SIGOA.Infrastructure.Email
{
    public interface IEmailService
    {
        void SendMail(string sender, string senderEmail, string mailTo, string mailcc, string subject, string body,
           string smtpServer, string fromEmail, string displayName, string userName, string password, int port, bool enableSsl);

        void SendMultMail(string sender, string senderEmail, string[] mailTo, string subject, string body,
           string smtpServer, string fromEmail, string displayName, string userName, string password, int port, bool enableSsl);
    }
}
