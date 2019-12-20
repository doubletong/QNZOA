using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class Role
    {
        public Role()
        {
            RoleMenus = new HashSet<RoleMenu>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsSys { get; set; }
        public string RoleName { get; set; }

        public ICollection<RoleMenu> RoleMenus { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
