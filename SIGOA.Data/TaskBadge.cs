using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class TaskBadge
    {
        public int TaskId { get; set; }
        public int BadgeId { get; set; }

        public Badge Badge { get; set; }
        public Task Task { get; set; }
    }
}
