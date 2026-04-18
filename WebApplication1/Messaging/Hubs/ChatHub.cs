using Messaging.Interfaces.Service;
using Messaging.Models.DTOs.Message;
using Microsoft.AspNetCore.SignalR;

namespace Messaging.Hubs;

public class ChatHub(IChatService chatService, ILogger<ChatHub> logger) : Hub
{
    private readonly IChatService _chatService = chatService;
    private readonly ILogger<ChatHub> _logger = logger;

    public async Task SendMessage(Guid chatId, string content)
    {
        try
        {
            _logger.LogInformation("Send message request came with " + content);
            var messageDto = new MessageCreateRequestDto
            {
                ChatId = chatId,
                EncryptedContent = content
            };

            var sentMessage = await _chatService.SendMessageAsync(messageDto);

            // Broadcast
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", sentMessage);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error sending message");
        }

    }

    public async Task JoinChat(Guid chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChat(Guid chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}
