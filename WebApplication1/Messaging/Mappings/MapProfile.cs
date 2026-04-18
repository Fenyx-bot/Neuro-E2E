using AutoMapper;
using Messaging.Models;
using Messaging.Models.DTOs.Auth;
using Messaging.Models.DTOs.Chat;
using Messaging.Models.DTOs.Message;
using Messaging.Models.DTOs.User;

namespace Messaging.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<AuthUser, SignUpDto>().ReverseMap();
        CreateMap<AuthUser, AuthUserDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Chat, ChatResponseDto>()
            .ForMember(dest => dest.User1Name, opt => opt.MapFrom(src => src.User1.DisplayName))
            .ForMember(dest => dest.User2Name, opt => opt.MapFrom(src => src.User2.DisplayName))
            .ReverseMap();
        CreateMap<Chat, ChatCreateRequestDto>().ReverseMap();
        CreateMap<Message, MessageResponseDto>()
            .ForMember(dest => dest.SenderUsername, opt => opt.MapFrom(src => src.Sender.DisplayName))
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.Sender.Id))
            .ReverseMap();
    }
}
