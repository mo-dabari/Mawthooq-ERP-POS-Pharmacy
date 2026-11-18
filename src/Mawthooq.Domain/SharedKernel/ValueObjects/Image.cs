using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.SharedKernel.ValueObjects
{
    public sealed record Image
    {
        public const int MaxStorageKeyLength = 500;
        public const int MaxFileNameLength = 100;
        public const int MaxMimeTypeLength = 50;

        public string StorageKey { get; }
        public string FileName { get; }
        public string MimeType { get; }

        private Image(string storageKey, string fileName, string mimeType)
        {
            StorageKey = storageKey;
            FileName = fileName;
            MimeType = mimeType;
        }

        public static Image Create(string storageKey, string fileName, string mimeType)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(storageKey, nameof(storageKey));
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName, nameof(fileName));
            ArgumentException.ThrowIfNullOrWhiteSpace(mimeType, nameof(mimeType));

            string key = storageKey.Trim(),
            name = fileName.Trim(),
            mime = mimeType.Trim();

            LengthGuard.MaxLength(key, MaxStorageKeyLength, nameof(storageKey));
            LengthGuard.MaxLength(name, MaxFileNameLength, nameof(fileName));
            LengthGuard.MaxLength(mime, MaxMimeTypeLength, nameof(mimeType));

            return new Image(key, name, mime);
        }

        public override string ToString() => FileName;
    }
}
