using System.Net;
using Xunit;

namespace Messaging.Tests;

public sealed class AuthorizationTests : IClassFixture<MessagingWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthorizationTests(MessagingWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Chats_endpoint_without_token_returns_unauthorized()
    {
        using var response = await _client.GetAsync("/chats");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Users_endpoint_without_token_returns_unauthorized()
    {
        using var response = await _client.GetAsync("/users");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
