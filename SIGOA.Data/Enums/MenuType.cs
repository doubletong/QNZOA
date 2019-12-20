using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIGOA.Data.Enums
{
    public enum MenuType:short
    {
        [Description("页面")]
        Page = 1,
        [Description("节点")]
        Node =2,
        [Description("操作")]
        Action = 3
    }
}
