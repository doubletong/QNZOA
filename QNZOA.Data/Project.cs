﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZOA.Data
{
    [Table("Project")]
    public partial class Project
    {
        public Project()
        {
            Paymentlogs = new HashSet<Paymentlog>();
            Tasks = new HashSet<Task>();
            UserProjects = new HashSet<UserProject>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        public int? CustomerId { get; set; }
        public Guid? Manager { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        public bool Archive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ArchiveDate { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Projects")]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(Manager))]
        [InverseProperty(nameof(User.Projects))]
        public virtual User ManagerNavigation { get; set; }
        [InverseProperty("Project")]
        public virtual ProjectBusiness ProjectBusiness { get; set; }
        [InverseProperty(nameof(Paymentlog.Project))]
        public virtual ICollection<Paymentlog> Paymentlogs { get; set; }
        [InverseProperty(nameof(Task.Project))]
        public virtual ICollection<Task> Tasks { get; set; }
        [InverseProperty(nameof(UserProject.Project))]
        public virtual ICollection<UserProject> UserProjects { get; set; }
    }
}