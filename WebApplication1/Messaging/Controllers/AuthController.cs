using System.Security.Claims;
using Messaging.Exceptions;
using Messaging.Interfaces.Service;
using Messaging.Models.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger)
    : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(SignUpDto signUpDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(signUpDto);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
        catch (UserExistsException e)
        {
            _logger.LogError(e.Message);
            return Conflict();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var result = await _authService.LoginAsync(loginDto);

            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            logger.LogError("Unauthorized access");
            return Unauthorized();
        }
        catch (InvalidPasswordException e)
        {
            logger.LogError(e.Message);
            return Unauthorized("Invalid password");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest();
        }
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _authService.GetCurrentUserAsync(new Guid(userId ?? string.Empty));

        return currentUser is null ? NotFound() : Ok(currentUser);
    }
}
