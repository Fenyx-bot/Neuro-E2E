using System.ComponentModel.DataAnnotations;

namespace Messaging.Models.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}