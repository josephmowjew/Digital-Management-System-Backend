using System;
namespace DataStore.Core.Models;

public class GenericSignature : Meta
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public string Tel { get; set; }
    public string Mobile { get; set; }
    public string Website { get; set; }
    public bool IsActive { get; set; } = true;
    
}
