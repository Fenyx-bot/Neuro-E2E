using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Messaging.Models.DTOs.Chat;

public class ChatCreateRequestDto
{
    [Required(ErrorMessage = "User1Id is required")]
    [JsonPropertyName("user1_id")]
    public Guid User1Id { get; set; }

    [Required(ErrorMessage = "User2Id is required")]
    [JsonPropertyName("user2_id")]
    public Guid User2Id { get; set; }
}