using System.ComponentModel.DataAnnotations;

namespace Messaging.Models;

public class Group: BaseEntity
{
    public string Name { get; set; }
    
    [Required]
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
    
    public ICollection<User> Users { get; set; }
    public ICollection<GroupMembership> Members { get; set; }
    public ICollection<GroupMessage> Messages { get; set; }
    
    public enum GroupType
    {
        Public,
        Private,
        Restricted
    }
    
    public GroupType Type { get; set; } = GroupType.Private;
    
    // TODO: Add Collection of Files
}