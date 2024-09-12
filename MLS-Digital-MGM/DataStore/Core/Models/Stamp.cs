namespace DataStore.Core.Models;

public class Stamp : Meta
{

    public string Name { get; set; }
    public int YearOfOperationId { get; set; }
    public YearOfOperation YearOfOperation { get; set; }

    public List<Attachment> Attachments {get; set; }

}