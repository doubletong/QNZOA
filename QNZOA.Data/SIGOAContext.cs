﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QNZOA.Data
{
    public partial class SIGOAContext : DbContext
    {
        public SIGOAContext()
        {
        }

        public SIGOAContext(DbContextOptions<SIGOAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<Badge> Badges { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<EmailAccount> EmailAccounts { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<LinkCategory> LinkCategories { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuCategory> MenuCategories { get; set; }
        public virtual DbSet<Paymentlog> Paymentlogs { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectBusiness> ProjectBusinesses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleMenu> RoleMenus { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskBadge> TaskBadges { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProject> UserProjects { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Article_ArticleCategory");
            });

            modelBuilder.Entity<ArticleCategory>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerType).HasComment("1：人个；2：公司");
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Links)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Link_LinkCategory");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_MenuSet_MenuCategorySet_CategoryId");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_MenuSet_MenuSet_ParentId");
            });

            modelBuilder.Entity<Paymentlog>(entity =>
            {
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Paymentlogs)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Paymentlog_Project");

                entity.HasOne(d => d.ProjectNavigation)
                    .WithMany(p => p.Paymentlogs)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Paymentlog_ProjectBusiness");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Archive).HasComment("是否存档");

                entity.Property(e => e.ArchiveDate).HasComment("存档时间");

                entity.Property(e => e.CustomerId).HasComment("客户");

                entity.Property(e => e.Description).HasComment("项目描述");

                entity.Property(e => e.EndDate).HasComment("结束时间");

                entity.Property(e => e.Manager).HasComment("项目负责人");

                entity.Property(e => e.Name).HasComment("项目名称");

                entity.Property(e => e.StartDate).HasComment("开始时间");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_ProjectSet_Customer");

                entity.HasOne(d => d.ManagerNavigation)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.Manager)
                    .HasConstraintName("FK_Project_User");
            });

            modelBuilder.Entity<ProjectBusiness>(entity =>
            {
                entity.Property(e => e.ProjectId).ValueGeneratedNever();

                entity.Property(e => e.Contract).HasComment("合同");

                entity.HasOne(d => d.Project)
                    .WithOne(p => p.ProjectBusiness)
                    .HasForeignKey<ProjectBusiness>(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectBusiness_ProjectSet");
            });

            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.MenuId })
                    .HasName("PK_RoleMenuSet");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.RoleMenus)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_RoleMenuSet_MenuSet_MenuId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleMenus)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleMenuSet_RoleSet_RoleId");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.FinishTme).HasComment("完成时间");

                entity.Property(e => e.Performer).HasComment("指派对象");

                entity.Property(e => e.StartTime).HasComment("开始时间");

                entity.Property(e => e.Status).HasComment("1未开始 2进行中 3完成  4已验收 5关闭");

                entity.HasOne(d => d.PerformerNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.Performer)
                    .HasConstraintName("FK_Task_User");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskSet_TaskSet");
            });

            modelBuilder.Entity<TaskBadge>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.BadgeId });

                entity.HasOne(d => d.Badge)
                    .WithMany(p => p.TaskBadges)
                    .HasForeignKey(d => d.BadgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskBadge_Badge");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskBadges)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskBadge_Task");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<UserProject>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.UserId });

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.UserProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject_UserProject");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProjects)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject_User");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_UserRoleSet");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserRoleSet_RoleSet_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRoleSet_UserSet_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}