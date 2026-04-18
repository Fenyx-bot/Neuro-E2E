using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messaging.Models;

public class Message: BaseEntity
{
    [Required]
    public Guid SenderId { get; set; }
    public User Sender { get; set; }
    
    [Required]
    public Guid RecipientId { get; set; }
    public User Recipient { get; set; }
    
    [Required]
    public Guid ChatId { get; set; }
    public Chat Chat { get; set; }
    
    [Required(ErrorMessage = "Message content is required")]
    [MaxLength(4000)]
    public string EncryptedContent { get; set; }
    
    public DateTime SentAt { get; set; } = DateTime.Now;
    public bool IsRead { get; set; } = false;
}