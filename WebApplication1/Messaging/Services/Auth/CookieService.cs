using Messaging.Interfaces.Service;

namespace Messaging.Services.Auth;

public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void Set(string token) => _httpContextAccessor.HttpContext?.Response.Cookies.Append("token", token,
        new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            MaxAge = TimeSpan.FromMinutes(30)
        });

    public string Get()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["token"];
        
        return string.IsNullOrEmpty(token) ? throw new Exception("User is not authenticated") : token;
    }

    public void Remove() => _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token");
}