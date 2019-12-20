using SIGOA.Data.Enums;
using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class User
    {
        public User()
        {
            Projects = new HashSet<Project>();
            Tasks = new HashSet<Task>();
            UserProjects = new HashSet<UserProject>();
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string PasswordHash { get; set; }
        public string PhotoUrl { get; set; }
        public string Qq { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime CreateDate { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
