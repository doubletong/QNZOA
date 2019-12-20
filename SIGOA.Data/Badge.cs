using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class Badge
    {
        public Badge()
        {
            TaskBadges = new HashSet<TaskBadge>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public int Importance { get; set; }

        public ICollection<TaskBadge> TaskBadges { get; set; }
    }
}
