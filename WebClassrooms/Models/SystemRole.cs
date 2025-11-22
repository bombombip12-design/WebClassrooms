namespace WebClassrooms.Models
{
    public class SystemRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        // Navigation
        public virtual ICollection<User> Users { get; set; }
    }
}
