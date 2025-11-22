namespace WebClassrooms.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual Class Class { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<AnnouncementFile> Files { get; set; }
    }
}
