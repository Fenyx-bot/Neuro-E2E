namespace Messaging.Models.DTOs.Auth;

public class AuthUserDto
{
    public Guid Id { get; set; } // Assuming BaseEntity has an Id property
    public string Username { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public int LoginAttempts { get; set; }
    public DateTime? LastLoginAttempt { get; set; }
}
