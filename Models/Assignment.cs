using System.ComponentModel.DataAnnotations;

namespace FinalASB.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        
        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự")]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Class Class { get; set; } = null!;
        public User Creator { get; set; } = null!;
        public ICollection<AssignmentFile> AssignmentFiles { get; set; } = new List<AssignmentFile>();
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

