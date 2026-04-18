using System.Text.Json.Serialization;
using Messaging.Models.DTOs.Message;

namespace Messaging.Models.DTOs.Chat;

public class ChatResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("user1_id")]
    public Guid User1Id { get; set; }
    [JsonPropertyName("user1_name")]
    public string User1Name { get; set; } // Optional: To include the username of User1
    [JsonPropertyName("user2_id")]
    public Guid User2Id { get; set; }
    [JsonPropertyName("user2_name")]
    public string User2Name { get; set; } // Optional: To include the username of User2
    [JsonPropertyName("last_message_at")]
    public DateTime LastMessageAt { get; set; }
}
