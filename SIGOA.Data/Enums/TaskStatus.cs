using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIGOA.Data.Enums
{
    public enum TaskStatus:short
    {
        [Description("未开始")]
        UnStart = 1,
        [Description("进行中")]
        Ongoing = 2,
        [Description("已完成")]
        Completed = 3,
        [Description("已验收")]
        Acceptanced = 4,
        [Description("关闭")]
        Closed = 10  
    }
}
