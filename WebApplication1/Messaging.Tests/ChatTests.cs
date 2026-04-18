using System.Net.Http.Json;
using System.Text.Json;
using Messaging.Models.DTOs.Auth;
using Messaging.Models.DTOs.Chat;
using Messaging.Models.DTOs.User;
using Xunit;

namespace Messaging.Tests;

public sealed class ChatTests : IClassFixture<MessagingWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _client;

    public ChatTests(MessagingWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_chat_then_list_chats_includes_it()
    {
        var password = "Password123!";

        var emailA = $"a{Guid.NewGuid():N}@example.com";
        var emailB = $"b{Guid.NewGuid():N}@example.com";

        await _client.RegisterAsync(new SignUpDto
        {
            Username = $"a{Guid.NewGuid():N}"[..12],
            Email = emailA,
            Password = password,
            DisplayName = "User A",
            ProfilePicture = null
        });

        await _client.RegisterAsync(new SignUpDto
        {
            Username = $"b{Guid.NewGuid():N}"[..12],
            Email = emailB,
            Password = password,
            DisplayName = "User B",
            ProfilePicture = null
        });

        var tokenA = await _client.LoginAsync(new LoginDto
        {
            Email = emailA,
            Password = password
        });

        _client.SetBearerToken(tokenA);

        var users = await _client.GetFromJsonAsync<List<UserDto>>("/users", JsonOptions);
        Assert.NotNull(users);

        var userA = users!.Single(u => u.DisplayName == "User A");
        var userB = users.Single(u => u.DisplayName == "User B");

        var created = await (await _client.PostAsJsonAsync("/chats", new ChatCreateRequestDto
        {
            User1Id = userA.Id,
            User2Id = userB.Id
        }, JsonOptions)).Content.ReadFromJsonAsync<ChatResponseDto>(JsonOptions);

        Assert.NotNull(created);
        Assert.Equal(userA.Id, created!.User1Id);
        Assert.Equal(userB.Id, created.User2Id);

        var chats = await _client.GetFromJsonAsync<List<ChatResponseDto>>("/chats", JsonOptions);
        Assert.NotNull(chats);
        Assert.Contains(chats!, c => c.Id == created.Id);
    }
}
