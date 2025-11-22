using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace WebClassrooms.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentFile> AssignmentFiles { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<SubmissionFile> SubmissionFiles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementFile> AnnouncementFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //=============== Users ===============
            modelBuilder.Entity<User>()
                .HasOne(u => u.SystemRole)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.SystemRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Owner)
                .WithMany(u => u.OwnedClasses)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            //=============== Enrollments ===============
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Class)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.UserId, e.ClassId })
                .IsUnique();

            //=============== Assignments ===============
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Class)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Creator)
                .WithMany(u => u.CreatedAssignments)
                .HasForeignKey(a => a.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            //=============== Assignment Files ===============
            modelBuilder.Entity<AssignmentFile>()
                .HasOne(af => af.Assignment)
                .WithMany(a => a.Files)
                .HasForeignKey(af => af.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            //=============== Submissions ===============
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Assignment)
                .WithMany(a => a.Submissions)
                .HasForeignKey(s => s.AssignmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Student)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Submission>()
                .HasIndex(s => new { s.AssignmentId, s.StudentId })
                .IsUnique();

            //=============== Submission Files ===============
            modelBuilder.Entity<SubmissionFile>()
                .HasOne(sf => sf.Submission)
                .WithMany(s => s.Files)
                .HasForeignKey(sf => sf.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            //=============== Comments ===============
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Class)
                .WithMany(cl => cl.Comments)
                .HasForeignKey(c => c.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Assignment)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AssignmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Submission)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.SubmissionId)
                .OnDelete(DeleteBehavior.Restrict);

            //=============== Announcements ===============
            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.Class)
                .WithMany(c => c.Announcements)
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.User)
                .WithMany(u => u.Announcements)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //=============== Announcement Files ===============
            modelBuilder.Entity<AnnouncementFile>()
                .HasOne(af => af.Announcement)
                .WithMany(a => a.Files)
                .HasForeignKey(af => af.AnnouncementId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

