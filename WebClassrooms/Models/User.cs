using System.Security.Claims;
using System.Xml.Linq;

namespace WebClassrooms.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string GoogleId { get; set; }
        public string AvatarUrl { get; set; }

        public int SystemRoleId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual SystemRole SystemRole { get; set; }
        public virtual ICollection<Class> OwnedClasses { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Assignment> CreatedAssignments { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}