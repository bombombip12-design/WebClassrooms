namespace FinalASB.Models
{
    public class AssignmentFile
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string DriveFileId { get; set; } = string.Empty;
        public string DriveFileUrl { get; set; } = string.Empty;

        // Navigation property
        public Assignment Assignment { get; set; } = null!;
    }
}

