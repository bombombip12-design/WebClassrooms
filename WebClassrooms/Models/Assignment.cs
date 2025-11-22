using System.Xml.Linq;

namespace WebClassrooms.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual Class Class { get; set; }
        public virtual User Creator { get; set; }
        public virtual ICollection<AssignmentFile> Files { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
