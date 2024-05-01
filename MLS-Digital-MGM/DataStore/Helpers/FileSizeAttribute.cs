using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Helpers;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class FileSizeAttribute : ValidationAttribute
{
    private readonly long _maxSize;

    public FileSizeAttribute(long maxSize)
    {
        _maxSize = maxSize;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var files = value as ICollection<IFormFile>;
        if (files != null)
        {
            foreach (var file in files)
            {
                if (file.Length > _maxSize)
                {
                    var maxSizeInMb = _maxSize / (1024 * 1024);
                    return new ValidationResult($"File size exceeds {maxSizeInMb}MB");
                    //return new ValidationResult($"File size exceeds {_maxSize} bytes");
                }
            }
        }
        return ValidationResult.Success;
    }
}