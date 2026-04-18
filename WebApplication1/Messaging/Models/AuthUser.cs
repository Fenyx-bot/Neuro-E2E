using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Models;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class AuthUser: BaseEntity
{
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50)]
    public string Username { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    public List<String> Roles { get; set; } = new List<string>();
    
    // Account status tracking
    public int LoginAttempts { get; set; } = 0;
    public DateTime? LastLoginAttempt { get; set; }
}