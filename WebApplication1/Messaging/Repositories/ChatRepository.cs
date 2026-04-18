using Messaging.DAL;
using Messaging.Interfaces.Repository;
using Messaging.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Messaging.Repositories;

public class ChatRepository(ApplicationDbContext context) : GenericRepository<Chat>(context), IChatRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Chat>> GetChatsByUserIdAsync(Guid userId)
    {
        return await _context.Chats
            .Where(chat => chat.User1Id == userId || chat.User2Id == userId)
            .OrderByDescending(chat => chat.LastMessageAt)
            .ToListAsync();
    }

    public async Task<Chat?> GetChatByUserIdsAsync(Guid user1Id, Guid user2Id)
    {
        return await _context.Chats
            .Where(chat => chat.User1Id == user1Id && chat.User2Id == user2Id)
            .FirstOrDefaultAsync();
    }
}
