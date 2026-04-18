using System.ComponentModel.DataAnnotations;

namespace Messaging.Models;

public class Chat : BaseEntity
{
    [Required]
    public Guid User1Id { get; set; }

    public User User1 { get; set; } = null;

    [Required]
    public Guid User2Id { get; set; }

    public User User2 { get; set; } = null;

    public ICollection<Message> Messages { get; set; }

    public DateTime LastMessageAt { get; set; }
}
