using System.Text.Json.Serialization;

namespace Messaging.Models.DTOs.User;

public class UserDto
{
  public Guid Id { get; set; }
  [JsonPropertyName("display_name")]
  public string DisplayName { get; set; }
  [JsonPropertyName("profile_picture")]
  public string ProfilePicture { get; set; }
  [JsonPropertyName("created_at")]
  public DateTime CreatedAt { get; set; }
}
