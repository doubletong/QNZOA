﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZOA.Data
{
    [Table("Badge")]
    public partial class Badge
    {
        public Badge()
        {
            TaskBadges = new HashSet<TaskBadge>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Color { get; set; }
        public int Importance { get; set; }

        [InverseProperty(nameof(TaskBadge.Badge))]
        public virtual ICollection<TaskBadge> TaskBadges { get; set; }
    }
}