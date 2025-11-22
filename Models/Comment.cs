namespace FinalASB.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ClassId { get; set; }
        public int? AssignmentId { get; set; }
        public int? SubmissionId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsPrivate { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User User { get; set; } = null!;
        public Class? Class { get; set; }
        public Assignment? Assignment { get; set; }
        public Submission? Submission { get; set; }
    }
}

