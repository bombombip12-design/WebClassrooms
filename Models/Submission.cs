namespace FinalASB.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public int? Score { get; set; }
        public string? TeacherComment { get; set; }

        // Navigation properties
        public Assignment Assignment { get; set; } = null!;
        public User Student { get; set; } = null!;
        public ICollection<SubmissionFile> SubmissionFiles { get; set; } = new List<SubmissionFile>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

