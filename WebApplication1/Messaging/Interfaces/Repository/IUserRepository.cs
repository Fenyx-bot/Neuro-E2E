using Messaging.Models;

namespace Messaging.Interfaces.Repository;

public interface IUserRepository: IGenericRepository<User>
{
    Task<User?> GetByAuthUserIdAsync(Guid authUserId);   
}