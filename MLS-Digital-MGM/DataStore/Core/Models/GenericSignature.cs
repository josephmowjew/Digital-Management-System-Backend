using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataStore.Core.Models;

public class GenericSignature : Meta
{
    public GenericSignature()
    {
        Attachments = new List<Attachment>();
    }

    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string CompanyName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Tel { get; set; }
    [Required]
    public string Mobile { get; set; }
    [Required]
    public string Website { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Collection navigation property
    public ICollection<Attachment> Attachments { get; set; }
}
