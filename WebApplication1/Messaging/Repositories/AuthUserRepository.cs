using Messaging.DAL;
using Messaging.Interfaces.Repository;
using Messaging.Models;

namespace Messaging.Repositories;

public class AuthUserRepository(ApplicationDbContext context) : GenericRepository<AuthUser>(context), IAuthUserRepository
{
    
}