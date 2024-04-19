﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Context
{
    public class TaskManagementContext:DbContext
    {

        public TaskManagementContext(DbContextOptions<TaskManagementContext> options) : base(options)
        {
        }

        public DbSet<User> User{ get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<SubTask> SubTask { get; set; }
        public DbSet<Tasks> Task { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Comment> Comment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
               .HasOne<User>()
               .WithMany()
               .HasForeignKey(p => p.ReportingManger);


            modelBuilder.Entity<User>()
               .HasOne<Role>()
               .WithMany()
               .HasForeignKey(p => p.RoleId);

            modelBuilder.Entity<SubTask>()
              .HasOne<Project>()
              .WithMany()
              .HasForeignKey(p => p.projectId);


             modelBuilder.Entity<Tasks>()
                .HasOne<User>()         
                .WithMany()
                .HasForeignKey(t => t.AssignTo)
                .IsRequired();

            modelBuilder.Entity<Tasks>()
                .HasOne<Status>()
                .WithMany()
                .HasForeignKey(t => t.Status)
                .IsRequired();

            modelBuilder.Entity<Tasks>()
                .HasOne<Priority>()
                .WithMany()
                .HasForeignKey(t => t.priority)
                .IsRequired();

            modelBuilder.Entity<Tasks>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.AssignBy)
                .IsRequired();

            modelBuilder.Entity<Tasks>()
                .HasOne<SubTask>()        
                .WithMany()
                .HasForeignKey(t => t.subTaskId);

            modelBuilder.Entity<Tasks>()
                .HasOne<Project>()        
                .WithMany()
                .HasForeignKey(t => t.ProjectId)
                .IsRequired();
            modelBuilder.Entity<Comment>()
               .HasOne<Tasks>()
               .WithMany()
               .HasForeignKey(t => t.TaskId)
               .IsRequired();
            modelBuilder.Entity<Comment>()
              .HasOne<User>()
              .WithMany()
              .HasForeignKey(t => t.UserId)
              .IsRequired();
            modelBuilder.Entity<Comment>()
              .HasOne<Comment>()
              .WithMany()
              .HasForeignKey(t => t.ReplyToCommentId);
        }
    }
}
