using System.Security.Claims;
using Messaging.Models;

namespace Messaging.Interfaces.Service;

public interface IJwtService
{
    public string GenerateToken(AuthUser user);
    public ClaimsPrincipal ValidateToken(string token);
}