using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messaging.Interfaces.Service;
using Messaging.Models;
using Microsoft.IdentityModel.Tokens;

namespace Messaging.Services.Auth;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(AuthUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(10),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero // Optional: reduce the clock skew
            }, out SecurityToken validatedToken);

            return claimsPrincipal;
        }
        catch (SecurityTokenExpiredException)
        {
            // Handle token expiration
            throw new SecurityTokenExpiredException("Token has expired.");
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            // Handle invalid signature
            throw new SecurityTokenInvalidSignatureException("Token has invalid signature.");
        }
        catch (Exception)
        {
            // Handle other exceptions
            throw new SecurityTokenException("Invalid token.");
        }
    }

}
