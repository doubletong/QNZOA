using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class UserRole
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}
