namespace WebClassrooms.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public string Role { get; set; } // Teacher | Student
        public DateTime JoinedAt { get; set; }

        // Navigation
        public virtual User User { get; set; }
        public virtual Class Class { get; set; }
    }
}
