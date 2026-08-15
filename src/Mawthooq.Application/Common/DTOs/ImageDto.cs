using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.DTOs
{
    public sealed record ImageDto(
         string StorageKey,
         string FileName,
         string MimeType
    );
}
