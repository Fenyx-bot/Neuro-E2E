using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Messaging.Models.DTOs.Auth;

public class SignUpDto
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Display name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Display name must be between 3 and 50 characters")]
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }
    
    public string? ProfilePicture { get; set; }
}