using System.Text.Json.Serialization;

namespace Messaging.Models.DTOs.Message;

public class MessageResponseDto
{
    public Guid Id { get; set; } // Message ID
    [JsonPropertyName("encrypted_content")]
    public string EncryptedContent { get; set; }
    [JsonPropertyName("sender_id")]
    public Guid SenderId { get; set; }
    [JsonPropertyName("sender_username")]
    public string SenderUsername { get; set; }
    [JsonPropertyName("sent_at")]
    public DateTime SentAt { get; set; }
}