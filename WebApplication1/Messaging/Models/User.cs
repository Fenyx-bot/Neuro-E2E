using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messaging.Models;

public class User: BaseEntity
{
    [Required]
    public Guid AuthUserId { get; set; }
    public AuthUser AuthUser { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string DisplayName { get; set; }
    [MaxLength(500)]
    public string ProfilePicture { get; set; }
    
    public bool IsOnline { get; set; } = false;
    public DateTime? LastSeen { get; set; }

    public ICollection<GroupMembership> GroupMemberships { get; set; } = new List<GroupMembership>();
    public ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public void UpdateLastSeen()
    {
        LastSeen = DateTime.Now;
        IsOnline = true;
    }
    
    public void MarkOffline()
    {
        IsOnline = false;
        LastSeen = DateTime.Now;
    }
}