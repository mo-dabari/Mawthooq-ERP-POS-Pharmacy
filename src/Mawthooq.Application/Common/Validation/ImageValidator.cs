using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Application.Common.Validation
{
    internal static class ImageValidator
{
    internal static void Validate(ImageDto image, string name, List<string> errors)
    {
        if (RequiredValidator.ValidateAndCollect(image, name, errors))
        {
            StringValidator.ValidateAndCollect(image.StorageKey, $"{name}.{nameof(image.StorageKey)}", Image.MaxStorageKeyLength, errors);
            StringValidator.ValidateAndCollect(image.FileName,   $"{name}.{nameof(image.FileName)}",   Image.MaxFileNameLength, errors);
            StringValidator.ValidateAndCollect(image.MimeType,   $"{name}.{nameof(image.MimeType)}",   Image.MaxMimeTypeLength, errors);
        }
    }
}
}
