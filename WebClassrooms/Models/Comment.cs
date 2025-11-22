namespace WebClassrooms.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ClassId { get; set; }
        public int? AssignmentId { get; set; }
        public int? SubmissionId { get; set; }
        public string Content { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual User User { get; set; }
        public virtual Class Class { get; set; }
        public virtual Assignment Assignment { get; set; }
        public virtual Submission Submission { get; set; }
    }
}
