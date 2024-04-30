using DataStore.Core.DTOs.Attachment;

namespace DataStore.Core.Mappers;

public class AttachmentProfile : AutoMapper.Profile
{
    public AttachmentProfile()
    {
        CreateMap<DataStore.Core.Models.Attachment, ReadAttachmentDTO>().ReverseMap();
    }
}