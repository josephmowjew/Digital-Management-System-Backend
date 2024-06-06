using System;
using System.IO;
namespace DataStore.Helpers;

public class FileNameGenerator
{
    public static string GenerateUniqueFileName(string originalFileName, int maxFileNameLength = 100)
    {
        // Generate a new GUID for uniqueness
        var uniqueId = Guid.NewGuid().ToString();

        // Extract the original file name without the extension
        var originalNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);

        // Extract the file extension
        var fileExtension = Path.GetExtension(originalFileName);

        // Limit the length of the original file name component
        var maxOriginalNameLength = maxFileNameLength - uniqueId.Length - fileExtension.Length - 2;
        if (maxOriginalNameLength < 0)
        {
            maxOriginalNameLength = 0;
        }

        var truncatedOriginalName = originalNameWithoutExtension.Length > maxOriginalNameLength
            ? originalNameWithoutExtension.Substring(0, maxOriginalNameLength)
            : originalNameWithoutExtension;

        // Construct the final unique file name
        var uniqueFileName = $"{uniqueId}_{truncatedOriginalName}{fileExtension}";

        return uniqueFileName;
    }
}