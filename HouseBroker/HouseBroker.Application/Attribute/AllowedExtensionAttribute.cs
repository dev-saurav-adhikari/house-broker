using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HouseBroker.Application.Attribute;

public class AllowedExtensionAttribute(string[] allowedExtensions) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var files = new List<IFormFile>();

        if (value is IFormFile singleFile)
        {
            files.Add(singleFile);
        }
        else if (value is IEnumerable<IFormFile> multipleFiles)
        {
            files.AddRange(multipleFiles);
        }
        else
        {
            var supportedFileType = string.Join(",", allowedExtensions);
            return new ValidationResult($"{ErrorMessage}. Supported file type: {supportedFileType} " ??
                                        "Invalid file extension");
        }

        foreach (var file in files)
        {
            var ext = Path.GetExtension(file.FileName);

            if (string.IsNullOrEmpty(ext) || !allowedExtensions.Contains(ext.ToLower()))
            {
                var supportedFileType = string.Join(",", allowedExtensions);
                return new ValidationResult($"{ErrorMessage}. Supported file type: {supportedFileType} " ??
                                            "Invalid file extension");
            }
        }

        return ValidationResult.Success;
    }
}