using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Messaging.Exceptions;
using Messaging.Interfaces.Repository;
using Messaging.Interfaces.Service;
using Messaging.Models;
using Messaging.Models.DTOs.Chat;
using Messaging.Models.DTOs.Message;
using Messaging.Repositories;

namespace Messaging.Services;

public class ChatService(
    IMapper mapper,
    IChatRepository chatRepository,
    IUserRepository userRepository,
    IMessageRepository messageRepository,
    ICurrentUser currentUser,
    ILogger<ChatService> logger,
    IConfiguration configuration) : IChatService
{
    private readonly IMapper _mapper = mapper;
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly ILogger<ChatService> _logger = logger;
    private readonly string key = configuration["Encryption:Key"]!;

    public async Task<ChatResponseDto?> CreateChatAsync(ChatCreateRequestDto chatRequestDto)
    {
        // Check if the user exists
        var user1 = await _userRepository.GetByIdAsync(chatRequestDto.User1Id);
        var user2 = await _userRepository.GetByIdAsync(chatRequestDto.User2Id);

        if (user1 == null || user2 == null)
        {
            _logger.LogWarning("User not found");
            throw new UserNotFoundException("User not found");
        }

        // Check if the chat already exists
        var chat = await _chatRepository.GetChatByUserIdsAsync(chatRequestDto.User1Id, chatRequestDto.User2Id);

        if (chat != null)
        {
            _logger.LogWarning("Chat already exists");
            return null;
        }

        // Create the chat
        chat = _mapper.Map<Chat>(chatRequestDto);
        chat = await _chatRepository.AddAsync(chat);

        return _mapper.Map<ChatResponseDto>(chat);
    }

    public async Task<List<ChatResponseDto>> GetAllChatsAsync()
    {
        var chats = await _chatRepository.GetAllAsync();
        return _mapper.Map<List<ChatResponseDto>>(chats);
    }

    public async Task<List<ChatResponseDto>> GetCurrentUsersChatsAsync()
    {
        var user = await _userRepository.GetByAuthUserIdAsync(
            _currentUser.GetCurrentUserId()
        );

        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        var chats = await _chatRepository.GetChatsByUserIdAsync(user.Id);
        List<ChatResponseDto> chatResponseDtos = [];

        foreach (var chat in chats)
        {
            var user1 = await _userRepository.GetByIdAsync(chat.User1Id);
            var user2 = await _userRepository.GetByIdAsync(chat.User2Id);

            if (user1 == null || user2 == null)
            {
                continue;
            }

            var chatResponseDto = _mapper.Map<ChatResponseDto>(chat);
            chatResponseDto.Name = user1.DisplayName + " and " + user2.DisplayName;

            chatResponseDtos.Add(chatResponseDto);
        }


        return chatResponseDtos;
    }

    public async Task<ChatResponseDto?> GetChatByIdAsync(Guid id)
    {
        var chat = await _chatRepository.GetByIdAsync(id);

        if (chat == null)
        {
            return null;
        }

        // Check if the user belongs to this chat
        var user = await _userRepository.GetByAuthUserIdAsync(
            _currentUser.GetCurrentUserId()
        );

        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        var userId = user.Id;
        if (chat.User1Id != userId && chat.User2Id != userId)
        {
            throw new UnauthorizedAccessException();
        }

        return _mapper.Map<ChatResponseDto>(chat);
    }

    public async Task<List<ChatResponseDto>> GetChatsByUserIdAsync(Guid userId)
    {
        // Check if user exists
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new UserNotFoundException($"User with {userId} not found");
        }

        var chats = await _chatRepository.GetChatsByUserIdAsync(userId);

        return _mapper.Map<List<ChatResponseDto>>(chats);
    }

    public async Task<List<MessageResponseDto>> GetMessagesAsync(Guid chatId)
    {
        var chat = await GetChatByIdAsync(chatId);

        if (chat == null)
        {
            throw new NotFoundException($"Chat with id: {chatId} is not found");
        }

        var messages = await _messageRepository.GetMessagesByChatIdAsync(chatId);

        // Encrypt the messages
        foreach (var message in messages)
        {
            string encryptedContent = EncryptMessage(message.EncryptedContent, key);
            message.EncryptedContent = encryptedContent;
        }

        return _mapper.Map<List<MessageResponseDto>>(messages);
    }

    public async Task<MessageResponseDto> SendMessageAsync(MessageCreateRequestDto messageRequestDto)
    {
        var transaction = await _chatRepository.BeginTransaction();
        try
        {
            // Retrieve the chat
            var chat = await GetChatByIdAsync(messageRequestDto.ChatId);
            if (chat == null)
            {
                throw new NotFoundException($"Chat with id: {messageRequestDto.ChatId} is not found");
            }

            // Decrypt the message content
            string encryptedContent = messageRequestDto.EncryptedContent;
            string decryptedContent = DecryptMessage(encryptedContent, key);

            _logger.LogInformation($"Decrypted message: {decryptedContent}");

            // Get the current user
            var user = await _userRepository.GetByAuthUserIdAsync(_currentUser.GetCurrentUserId());
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            // Determine the recipient
            Guid recipientId = chat.User1Id == user.Id ? chat.User2Id : chat.User1Id;

            // Create the message
            var message = new Message
            {
                ChatId = messageRequestDto.ChatId,
                SenderId = user.Id,
                RecipientId = recipientId,
                EncryptedContent = decryptedContent,
                SentAt = DateTime.UtcNow
            };

            // Save the message
            message = await _messageRepository.AddAsync(message);

            var chatEntity = await _chatRepository.GetByIdAsync(chat.Id);

            if (chatEntity != null)
            {
                chatEntity.LastMessageAt = message.SentAt;
                await _chatRepository.UpdateAsync(chatEntity);
            }

            // Commit the transaction
            await transaction.CommitAsync();

            message.EncryptedContent = EncryptMessage(message.EncryptedContent, key);

            // Return the response
            return _mapper.Map<MessageResponseDto>(message);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public Task<bool> DeleteChatAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateChatAsync(Guid id, ChatCreateRequestDto chatRequestDto)
    {
        throw new NotImplementedException();
    }


    private string EncryptMessage(string message, string secretKey)
    {
        try
        {
            // Decode the Base64 secret key into a byte array
            byte[] key = Convert.FromBase64String(secretKey);

            // Generate a random 16-byte IV
            byte[] iv = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }

            // Convert the message to a byte array
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            // Create an AES instance
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Encrypt the message
                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedMessage = encryptor.TransformFinalBlock(messageBytes, 0, messageBytes.Length);

                    // Combine the IV and encrypted message
                    byte[] combined = new byte[iv.Length + encryptedMessage.Length];
                    Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
                    Buffer.BlockCopy(encryptedMessage, 0, combined, iv.Length, encryptedMessage.Length);

                    // Base64 encode the combined result
                    return Convert.ToBase64String(combined);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Encryption error: " + ex.Message);
            throw new Exception("Failed to encrypt the message", ex);
        }
    }

    private string DecryptMessage(string encryptedMessage, string secretKey)
    {
        try
        {
            // Convert the base64 encoded secret key to bytes
            byte[] key = Convert.FromBase64String(secretKey);

            // Convert the encrypted message to bytes
            byte[] cipherBytes = Convert.FromBase64String(encryptedMessage);

            // Extract the IV (first 16 bytes)
            byte[] iv = new byte[16];
            Array.Copy(cipherBytes, 0, iv, 0, 16);

            // Extract the actual encrypted content (remaining bytes)
            byte[] encryptedContent = new byte[cipherBytes.Length - 16];
            Array.Copy(cipherBytes, 16, encryptedContent, 0, encryptedContent.Length);

            // Use AES decryption
            using Aes aesAlg = Aes.Create();
            // Configure the algorithm
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            // Create a decryptor
            using ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            // Decrypt the message
            using MemoryStream msDecrypt = new(encryptedContent);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            // Read the decrypted message
            return srDecrypt.ReadToEnd();
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            Console.WriteLine($"Decryption error: {ex.Message}");
            throw; // It's better to throw the exception for debugging
        }
    }
}
