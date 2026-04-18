using System.Security.Claims;
using Messaging.Interfaces.Repository;
using Messaging.Interfaces.Service;
using Messaging.Models;

namespace Messaging.Services.Auth;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity!.IsAuthenticated)
        {
            throw new UnauthorizedAccessException();
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier);
        return userId != null ? Guid.Parse(userId.Value) : throw new UnauthorizedAccessException();
    }
}