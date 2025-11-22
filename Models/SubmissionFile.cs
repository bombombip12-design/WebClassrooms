namespace FinalASB.Models
{
    public class SubmissionFile
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string DriveFileId { get; set; } = string.Empty;
        public string DriveFileUrl { get; set; } = string.Empty;

        // Navigation property
        public Submission Submission { get; set; } = null!;
    }
}

