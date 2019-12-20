using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIGOA.Data.Enums
{
    public enum Gender:short
    {
        [Description("无")]
        Unknown = 0,
        [Description("男")]
        Male = 1,
        [Description("女")]
        Female = 2
       
         
    }
}
