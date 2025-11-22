namespace FinalASB.Models
{
    public class SystemRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        
        // Navigation property
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

