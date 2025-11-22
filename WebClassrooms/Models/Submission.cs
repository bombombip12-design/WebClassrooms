using System.Xml.Linq;

namespace WebClassrooms.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int? Score { get; set; }
        public string TeacherComment { get; set; }

        // Navigation
        public virtual Assignment Assignment { get; set; }
        public virtual User Student { get; set; }
        public virtual ICollection<SubmissionFile> Files { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
