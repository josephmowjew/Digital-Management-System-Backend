using AutoMapper;
using DataStore.Core.DTOs.Message;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<CreateMessageDTO, Message>();
            CreateMap<Message, ReadMessageDTO>();
            CreateMap<UpdateMessageDTO, Message>();
        }
    }
}
