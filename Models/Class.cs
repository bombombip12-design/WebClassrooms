namespace FinalASB.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ClassUrl { get; set; }
        public string JoinCode { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public string? CoverImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User Owner { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    }
}

