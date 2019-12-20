using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SIGOA.Model
{
    public class EmailVM
    {
    }
    #region Email Account

    public class EmailAccountVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Smtpserver { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool IsDefault { get; set; }

    }
    public class EmailAccountPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public IList<EmailAccountVM> ItemList { get; set; }

        public int LastPageIndex
        {
            get
            {
                if (PageSize > 0)
                {
                    var d = RowCount / PageSize;
                    return (int)Math.Floor((double)d);
                }
                return 0;
            }
        }
    }

    public class EmailAccountIM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Smtpserver { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool IsDefault { get; set; }

    }
    public class TestEmailIM
    {
        //[Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int AccountId { get; set; }
   
        [Required]
        [EmailAddress]
        public string TestEmail { get; set; }
    }
    #endregion
}
