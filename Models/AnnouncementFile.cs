namespace FinalASB.Models
{
    public class AnnouncementFile
    {
        public int Id { get; set; }
        public int AnnouncementId { get; set; }
        public string DriveFileId { get; set; } = string.Empty;
        public string DriveFileUrl { get; set; } = string.Empty;

        // Navigation property
        public Announcement Announcement { get; set; } = null!;
    }
}

