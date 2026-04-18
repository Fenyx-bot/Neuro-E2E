using Messaging.Models;

namespace Messaging.Interfaces.Repository;

public interface IChatRepository: IGenericRepository<Chat>
{   
    Task<List<Chat>> GetChatsByUserIdAsync(Guid userId);
    Task<Chat?> GetChatByUserIdsAsync(Guid user1Id, Guid user2Id);
}