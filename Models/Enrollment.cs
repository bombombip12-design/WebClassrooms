namespace FinalASB.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public string Role { get; set; } = "Student"; // Teacher or Student
        public DateTime JoinedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User User { get; set; } = null!;
        public Class Class { get; set; } = null!;
    }
}

