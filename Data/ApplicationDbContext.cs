using Microsoft.EntityFrameworkCore;
using FinalASB.Models;

namespace FinalASB.Data
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

            // Configure User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure Enrollment unique constraint
            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.UserId, e.ClassId })
                .IsUnique();

            // Configure Submission unique constraint
            modelBuilder.Entity<Submission>()
                .HasIndex(s => new { s.AssignmentId, s.StudentId })
                .IsUnique();

            // Configure Enrollment Role check constraint (handled in application logic)
            modelBuilder.Entity<Enrollment>()
                .Property(e => e.Role)
                .HasMaxLength(20);

            // Configure Assignment - map Creator navigation to CreatedBy foreign key
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Creator)
                .WithMany(u => u.CreatedAssignments)
                .HasForeignKey(a => a.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Class - map Owner navigation to OwnerId foreign key
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Owner)
                .WithMany(u => u.OwnedClasses)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Submission - map Student navigation to StudentId foreign key
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Student)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

