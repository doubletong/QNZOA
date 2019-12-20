using System;
using System.Collections.Generic;
using System.Text;

namespace SIGOA.Infrastructure.Helpers
{
    public static class Common
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
