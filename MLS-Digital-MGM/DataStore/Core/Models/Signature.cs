namespace DataStore.Core.Models;

public class Signature : Meta
{

    public string Name { get; set; }
    public int YearOfOperationId { get; set; }
    public YearOfOperation YearOfOperation { get; set; }
    public string CreatedById { get; set; }
    public ApplicationUser CreatedBy { get; set; } 
    public List<Attachment> Attachments {get; set; }

}