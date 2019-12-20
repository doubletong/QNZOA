using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace QNZOA.Model
{
    public class ReturnVM
    {      
        public ReturnVM()
        {
            Status = false;
        }
        public HttpStatusCode? HttpStatusCode { get; set; }
        public bool Status { get; set; }
        public string Data { get; set; }
        public string Message { get; set; }

    }
}
