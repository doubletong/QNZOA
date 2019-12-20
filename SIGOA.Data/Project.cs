using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class Project
    {
        public Project()
        {
            Paymentlogs = new HashSet<Paymentlog>();
            Tasks = new HashSet<Task>();
            UserProjects = new HashSet<UserProject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? CustomerId { get; set; }
        public Guid? Manager { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Archive { get; set; }
        public DateTime? ArchiveDate { get; set; }

        public Customer Customer { get; set; }
        public User ManagerNavigation { get; set; }
        public ProjectBusiness ProjectBusiness { get; set; }
        public ICollection<Paymentlog> Paymentlogs { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
    }
}
