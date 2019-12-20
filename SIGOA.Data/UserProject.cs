using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class UserProject
    {
        public int ProjectId { get; set; }
        public Guid UserId { get; set; }

        public Project Project { get; set; }
        public User User { get; set; }
    }
}
