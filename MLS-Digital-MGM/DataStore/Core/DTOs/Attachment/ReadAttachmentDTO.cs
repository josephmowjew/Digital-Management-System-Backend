using DataStore.Core.Models;

namespace DataStore.Core.DTOs.Attachment;

public class ReadAttachmentDTO
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public AttachmentType AttachmentType{ get; set; }

    public string AbsoluteFilePath { get; set; }
}