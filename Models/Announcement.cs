namespace FinalASB.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Class Class { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<AnnouncementFile> AnnouncementFiles { get; set; } = new List<AnnouncementFile>();
    }
}

