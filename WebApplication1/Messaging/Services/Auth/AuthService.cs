using AutoMapper;
using Messaging.Exceptions;
using Messaging.Interfaces.Repository;
using Messaging.Interfaces.Service;
using Messaging.Models;
using Messaging.Models.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace Messaging.Services.Auth;

public class AuthService(
    IMapper mapper,
    IAuthUserRepository authUserRepository,
    IUserRepository userRepository,
    IJwtService jwtService,
    ICookieService cookieService,
    ICurrentUser currentUser,
    ILogger<AuthService> logger
    ) : IAuthService
{
    private readonly IMapper _mapper = mapper;
    private readonly IAuthUserRepository _authUserRepository = authUserRepository;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICookieService _cookieService = cookieService;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<bool> RegisterAsync(SignUpDto signUpDto)
    {
        var emailExists = await _authUserRepository.AnyAsync(x => x.Email == signUpDto.Email);

        if (emailExists)
        {
            throw new UserExistsException("User with this email already exists");
        }

        var usernameExists = await _authUserRepository.AnyAsync(x => x.Username == signUpDto.Username);

        if (usernameExists)
        {
            throw new UserExistsException("User with this username already exists");
        }

        var authUser = _mapper.Map<AuthUser>(signUpDto);
        authUser.Password = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password);
        authUser.Roles = new List<string>(
            new string[] { "User" }
        );
        await _authUserRepository.AddAsync(authUser);

        // Create the user associated with the auth user
        var user = new User
        {
            AuthUserId = authUser.Id,
            DisplayName = signUpDto.DisplayName,
            ProfilePicture = signUpDto.ProfilePicture ?? "https://www.gravatar.com/avatar/"
        };
        await _userRepository.AddAsync(user);

        return true;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _authUserRepository.FirstOrDefaultAsync(x => x.Email == loginDto.Email) ??
                   throw new UserNotFoundException("User not found");

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            logger.LogError("Invalid password");
            user.LoginAttempts++;
            user.LastLoginAttempt = DateTime.Now;
            logger.LogInformation($"Login attempts: {user.LoginAttempts}");
            
            await _authUserRepository.UpdateAsync(user);
            
            throw new InvalidPasswordException("Invalid password");
        }

        user.LoginAttempts = 0;
        user.LastLoginAttempt = null;
        await _authUserRepository.UpdateAsync(user);
        
        var token = _jwtService.GenerateToken(user);
        _cookieService.Set(token);

        return token;
    }

    public void Logout()
    {
        try
        {
            _ = _cookieService.Get();
            _cookieService.Remove();
        }
        catch
        {
        }
    }

    public async Task<AuthUserDto?> GetCurrentUserAsync()
    {
        var userId = _currentUser.GetCurrentUserId();
        var user = await _authUserRepository.GetByIdAsync(userId);
        
        return _mapper.Map<AuthUserDto>(user);
    }
    
    public async Task<AuthUserDto?> GetCurrentUserAsync(Guid id)
    {
        AuthUser? user = await _authUserRepository.GetByIdAsync(id);
        
        return _mapper.Map<AuthUserDto>(user);
    }
}