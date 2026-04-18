using System.ComponentModel.DataAnnotations;

namespace Messaging.Models;

public class GroupMessage: BaseEntity
{
    [Required]
    public Guid SenderId { get; set; }
    public User Sender { get; set; }
    
    [Required]
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    
    [Required(ErrorMessage = "Message content is required")]
    [MaxLength(4000)]
    public string EncryptedContent { get; set; }
    
    public DateTime SentAt { get; set; } = DateTime.Now;
    
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
}