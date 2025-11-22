namespace FinalASB.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }
        public string? GoogleId { get; set; }
        public string? AvatarUrl { get; set; }
        public int SystemRoleId { get; set; } = 2; // Default to User role
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public SystemRole SystemRole { get; set; } = null!;
        public ICollection<Class> OwnedClasses { get; set; } = new List<Class>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Assignment> CreatedAssignments { get; set; } = new List<Assignment>();
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    }
}

