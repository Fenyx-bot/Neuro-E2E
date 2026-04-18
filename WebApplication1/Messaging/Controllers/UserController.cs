using Messaging.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserByIdAsync(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var user = await _userService.GetCurrentUserAsync();
        return user is null ? NotFound() : Ok(user);
    }
}
