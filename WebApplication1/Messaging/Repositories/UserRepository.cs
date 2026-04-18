using Messaging.DAL;
using Messaging.Interfaces.Repository;
using Messaging.Models;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Repositories;

public class UserRepository(ApplicationDbContext context) : GenericRepository<User>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User?> GetByAuthUserIdAsync(Guid authUserId)
    {
        return await _context.Users.FirstOrDefaultAsync(
            user => user.AuthUserId == authUserId
        );
    }
}