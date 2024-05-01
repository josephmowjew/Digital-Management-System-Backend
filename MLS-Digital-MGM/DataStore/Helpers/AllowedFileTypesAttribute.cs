using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Helpers;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class AllowedFileTypesAttribute : ValidationAttribute
{
    private readonly string[] _allowedTypes;

    public AllowedFileTypesAttribute(string[] allowedTypes)
    {
        _allowedTypes = allowedTypes;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var files = value as ICollection<IFormFile>;
        if (files != null)
        {
            foreach (var file in files)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                if (!_allowedTypes.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                {
                    return new ValidationResult("File type not allowed. Please make sure it is a document");
                }
            }
        }
        return ValidationResult.Success;
    }
}