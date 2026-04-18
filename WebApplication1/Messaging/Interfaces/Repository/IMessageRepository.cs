using Messaging.Models;

namespace Messaging.Interfaces.Repository;

public interface IMessageRepository: IGenericRepository<Message>
{
    Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId);
}