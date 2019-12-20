using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class RoleMenu
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }

        public Menu Menu { get; set; }
        public Role Role { get; set; }
    }
}
