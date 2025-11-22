namespace WebClassrooms.Models
{
    public class AnnouncementFile
    {
        public int Id { get; set; }
        public int AnnouncementId { get; set; }
        public string DriveFileId { get; set; }
        public string DriveFileUrl { get; set; }

        // Navigation
        public virtual Announcement Announcement { get; set; }
    }
}
