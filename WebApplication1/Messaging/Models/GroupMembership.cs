using System.ComponentModel.DataAnnotations;

namespace Messaging.Models;

public class GroupMembership: BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    [Required]
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    
    public DateTime JoinedAt { get; set; } = DateTime.Now;

    public enum MemberRole
    {
        Member,
        Admin,
        Owner
    }
    
    public MemberRole Role { get; set; } = MemberRole.Member;
}