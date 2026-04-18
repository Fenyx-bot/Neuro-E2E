using Messaging.Models;
using Messaging.Models.DTOs.User;

namespace Messaging.Interfaces.Service;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto?> GetCurrentUserAsync();
}
