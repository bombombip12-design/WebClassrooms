using System.Xml.Linq;

namespace WebClassrooms.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public string ClassUrl { get; set; }
        public string JoinCode { get; set; }
        public int OwnerId { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual User Owner { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
