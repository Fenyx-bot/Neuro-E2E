using Messaging.DAL;
using Messaging.Interfaces.Repository;
using Messaging.Models;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Repositories;

public class MessageRepository(ApplicationDbContext context) : GenericRepository<Message>(context), IMessageRepository
{
    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        return await context.Messages
            .Include(msg => msg.Sender)
            .Where(msg => msg.ChatId == chatId)
            .ToListAsync();
    }
}
