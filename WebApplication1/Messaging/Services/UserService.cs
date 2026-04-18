using AutoMapper;
using Messaging.Interfaces.Repository;
using Messaging.Interfaces.Service;
using Messaging.Models;
using Messaging.Models.DTOs.User;

namespace Messaging.Services;

public class UserService(IMapper mapper, IUserRepository userRepository, ICurrentUser currentUser) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var userId = _currentUser.GetCurrentUserId();
        var user = await _userRepository.GetByAuthUserIdAsync(userId);

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        return _mapper.Map<UserDto>(user);
    }
}
