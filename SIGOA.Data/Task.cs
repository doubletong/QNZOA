using SIGOA.Data.Enums;
using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class Task
    {
        public Task()
        {
            TaskBadges = new HashSet<TaskBadge>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Guid? Performer { get; set; }
        public TaskStatus Status { get; set; }
        public int ProjectId { get; set; }
        public decimal WorkHours { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTme { get; set; }

        public User PerformerNavigation { get; set; }
        public Project Project { get; set; }
        public ICollection<TaskBadge> TaskBadges { get; set; }
    }
}
