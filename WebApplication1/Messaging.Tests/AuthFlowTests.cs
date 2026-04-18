using Messaging.Models.DTOs.Auth;
using Xunit;

namespace Messaging.Tests;

public sealed class AuthFlowTests : IClassFixture<MessagingWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthFlowTests(MessagingWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_then_login_returns_jwt_token()
    {
        var email = $"user{Guid.NewGuid():N}@example.com";
        var password = "Password123!";

        await _client.RegisterAsync(new SignUpDto
        {
            Username = $"u{Guid.NewGuid():N}"[..12],
            Email = email,
            Password = password,
            DisplayName = "Test User",
            ProfilePicture = null
        });

        var token = await _client.LoginAsync(new LoginDto
        {
            Email = email,
            Password = password
        });

        // Basic JWT shape check: header.payload.signature
        Assert.Equal(3, token.Split('.').Length);
    }
}
