using System.ComponentModel.DataAnnotations;

namespace WebClassrooms.Models.ViewModels
{
    public class RegisterVm
    {
        [Required] public string FullName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required, MinLength(6)] public string Password { get; set; }
        [Compare("Password")] public string ConfirmPassword { get; set; }
    }
}
