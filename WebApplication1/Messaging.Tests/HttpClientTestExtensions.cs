using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Messaging.Models.DTOs.Auth;

namespace Messaging.Tests;

internal static class HttpClientTestExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static void SetBearerToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public static async Task RegisterAsync(this HttpClient client, SignUpDto dto)
    {
        using var response = await client.PostAsJsonAsync("/auth/register", dto, JsonOptions);
        response.EnsureSuccessStatusCode();
    }

    public static async Task<string> LoginAsync(this HttpClient client, LoginDto dto)
    {
        using var response = await client.PostAsJsonAsync("/auth/login", dto, JsonOptions);
        response.EnsureSuccessStatusCode();

        // The API returns a raw JWT string (often as text/plain), not necessarily JSON.
        var token = (await response.Content.ReadAsStringAsync()).Trim();
        if (token.Length >= 2 && token[0] == '"' && token[^1] == '"')
        {
            token = token[1..^1];
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("Login did not return a token.");
        }

        return token;
    }
}
