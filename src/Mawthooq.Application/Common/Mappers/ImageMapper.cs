using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Application.Common.Mappers
{
    public static class ImageMapper
    {
        public static Image ToDomain(this ImageDto imageDto)
        {
            ArgumentNullException.ThrowIfNull(imageDto, nameof(imageDto));

            return Image.Create(imageDto.StorageKey, imageDto.FileName, imageDto.MimeType);
        }

        public static ImageDto ToDto(this Image image)
        {
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            return new ImageDto(image.StorageKey, image.FileName, image.MimeType);
        }
    }
}
