using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Messaging.Models.DTOs.Message;

public class MessageCreateRequestDto
{
    public Guid ChatId { get; set; }
    [Required(ErrorMessage = "The message is required")]
    [JsonPropertyName("encrypted_content")]
    public string EncryptedContent { get; set; }
}