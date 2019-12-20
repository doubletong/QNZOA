using System;
using System.Collections.Generic;
using System.Text;

namespace SIGOA.Model
{
    public class RoleVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsSys { get; set; }
        public string RoleName { get; set; }
        public int UserCount { get; set; }
    }

    public class RoleIM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsSys { get; set; }
        public string RoleName { get; set; }
    }

    public class SetRoleMenusIM
    {
        public int RoleId { get; set; }
        public List<RoleMenusIM> Menus { get; set; }
    }

}
