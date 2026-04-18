using Messaging.Models.DTOs.Auth;

namespace Messaging.Interfaces.Service;

public interface IAuthService
{
    Task<bool> RegisterAsync(SignUpDto signUpDto);
    Task<string> LoginAsync(LoginDto loginDto);
    void Logout();
    Task<AuthUserDto?> GetCurrentUserAsync();
    Task<AuthUserDto?> GetCurrentUserAsync(Guid id);
}
