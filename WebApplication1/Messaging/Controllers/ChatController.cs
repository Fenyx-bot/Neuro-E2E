using Messaging.Exceptions;
using Messaging.Interfaces.Service;
using Messaging.Models.DTOs.Chat;
using Messaging.Models.DTOs.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Controllers;

[ApiController]
[Route("chats")]
[Authorize]
public class ChatController(IChatService chatService) : ControllerBase
{
    private readonly IChatService _chatService = chatService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChat(Guid id)
    {
        try
        {
            var chat = await _chatService.GetChatByIdAsync(id);

            if (chat == null)
            {
                return NotFound();
            }

            return Ok(chat);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllChats()
    {
        try
        {
            var chats = await _chatService.GetCurrentUsersChatsAsync();
            return Ok(chats);
        } 
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateChat(ChatCreateRequestDto chatRequestDto)
    {
        try
        {
            var chat = await _chatService.CreateChatAsync(chatRequestDto);

            if (chat == null)
            {
                return BadRequest("Chat already exists");
            }

            return Ok(chat);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}/messages")]
    public async Task<IActionResult> GetMessages(Guid id)
    {
        try
        {
            var messages = await _chatService.GetMessagesAsync(id);

            return Ok(messages);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{id}/messages")]
    public async Task<IActionResult> SendMessage(Guid id, MessageCreateRequestDto messageRequestDto)
    {
        try
        {
            messageRequestDto.ChatId = id;
            var message = await _chatService.SendMessageAsync(messageRequestDto);
            return Ok(message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}