namespace WebClassrooms.Models
{
    public class AssignmentFile
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string DriveFileId { get; set; }
        public string DriveFileUrl { get; set; }

        // Navigation
        public virtual Assignment Assignment { get; set; }
    }
}
