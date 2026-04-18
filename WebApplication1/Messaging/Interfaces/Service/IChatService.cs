using Messaging.Models.DTOs.Chat;
using Messaging.Models.DTOs.Message;

namespace Messaging.Interfaces.Service;

public interface IChatService
{
    Task<ChatResponseDto?> CreateChatAsync(ChatCreateRequestDto chatRequestDto);
    Task<List<ChatResponseDto>> GetAllChatsAsync();
    Task<List<ChatResponseDto>> GetCurrentUsersChatsAsync();
    Task<ChatResponseDto?> GetChatByIdAsync(Guid id);
    Task<bool> DeleteChatAsync(Guid id);
    Task<bool> UpdateChatAsync(Guid id, ChatCreateRequestDto chatRequestDto);
    Task<List<ChatResponseDto>> GetChatsByUserIdAsync(Guid userId);
    Task<List<MessageResponseDto>> GetMessagesAsync(Guid chatId);
    Task<MessageResponseDto> SendMessageAsync(MessageCreateRequestDto messageRequestDto);
}
