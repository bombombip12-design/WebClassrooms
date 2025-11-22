namespace WebClassrooms.Models
{
    public class SubmissionFile
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string DriveFileId { get; set; }
        public string DriveFileUrl { get; set; }

        // Navigation
        public virtual Submission Submission { get; set; }
    }
}
