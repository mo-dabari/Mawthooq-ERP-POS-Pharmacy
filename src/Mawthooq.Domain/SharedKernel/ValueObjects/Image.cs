using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.ValueObjects
{

    public sealed record Image
    {
        public string StorageKey { get; }
        public string FileName { get; }
        public string MimeType { get; }

        private Image(
            string storageKey,
            string fileName,
            string mimeType)
        {
            StorageKey = storageKey;
            FileName = fileName;
            MimeType = mimeType;
        }

        public static Image Create(
            string storageKey,
            string fileName,
            string mimeType)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(storageKey, nameof(storageKey));
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName, nameof(fileName));
            ArgumentException.ThrowIfNullOrWhiteSpace(mimeType, nameof(mimeType));

            return new Image(
                storageKey.Trim(),
                fileName.Trim(),
                mimeType.Trim());
        }

        public override string ToString() => FileName;
    }


















    // public sealed record Image
    // {
    //     public const long MaxSizeBytes = 5 * 1024 * 1024; // 5 MB

    //     private static readonly Dictionary<string, string> AllowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    //     {
    //         { ".jpg",  "image/jpeg" },
    //         { ".jpeg", "image/jpeg" },
    //         { ".png",  "image/png" },
    //         { ".webp", "image/webp" }
    //     };

    //     private static readonly Dictionary<string, List<byte[]>> Signatures = new(StringComparer.OrdinalIgnoreCase)
    //     {
    //         {
    //             ".jpg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } }
    //         },
    //         {
    //             ".jpeg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } }
    //         },
    //         {
    //             ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } }
    //         },
    //         {
    //             ".webp", new List<byte[]> { new byte[] { 0x52, 0x49, 0x46, 0x46 } } // "RIFF"
    //         }
    //     };

    //     public string FileName { get; }
    //     public string MimeType { get; }
    //     public long FileSizeBytes { get; }

    //     private ImageFile(string fileName, string mimeType, long fileSizeBytes)
    //     {
    //         FileName = fileName;
    //         MimeType = mimeType;
    //         FileSizeBytes = fileSizeBytes;
    //     }

    //     public static ImageFile Create(string fileName, Stream fileStream)
    //     {
    //         if (string.IsNullOrWhiteSpace(fileName))
    //             throw new InvalidOperationException("The image file name cannot be empty");

    //         fileName = fileName.Trim();
    //         var extension = Path.GetExtension(fileName);

    //         if (fileStream == null || fileStream.Length == 0)
    //             throw new InvalidOperationException(" The uploaded file is empty");

    //         if (string.IsNullOrEmpty(extension) || !AllowedMimeTypes.ContainsKey(extension))
    //         {
    //             var allowedList = string.Join(", ", AllowedMimeTypes.Keys);
    //             throw new InvalidOperationException($"File extension not supported ({extension}). Allowed extensions: {allowedList}");
    //         }

    //         if (fileStream.Length > MaxSizeBytes)
    //         {
    //             var maxSizeMb = MaxSizeBytes / (1024 * 1024);
    //             throw new InvalidOperationException($"The image size exceeds the maximum allowed limit ({maxSizeMb} MB).");
    //         }

    //         if (!IsValidImageSignature(fileStream, extension))
    //             throw new InvalidOperationException("The file content does not match its extension! The file is corrupt or the upload attempt is unsafe.");

    //         var mimeType = AllowedMimeTypes[extension];

    //         return new ImageFile(fileName, mimeType, fileStream.Length);
    //     }

    //     private static bool IsValidImageSignature(Stream stream, string extension)
    //     {
    //         if (stream == null || stream.Length == 0)
    //             return false;

    //         if (string.IsNullOrWhiteSpace(extension) || !Signatures.ContainsKey(extension))
    //             return false;

    //         var allowedSignatures = Signatures[extension];

    //         int maxBytesToRead = extension.Equals(".webp", StringComparison.OrdinalIgnoreCase)
    //             ? 12
    //             : allowedSignatures.Max(s => s.Length);
    //         byte[] headerBytes = new byte[maxBytesToRead];

    //         long originalPosition = stream.Position;

    //         try
    //         {
    //             stream.Position = 0;
    //             int bytesRead = stream.Read(headerBytes, 0, maxBytesToRead);

    //             if (bytesRead < maxBytesToRead)
    //                 return false;

    //             if (extension.Equals(".webp", StringComparison.OrdinalIgnoreCase))
    //             {
    //                 var riffSignature = new byte[] { 0x52, 0x49, 0x46, 0x46 }; // "RIFF"
    //                 var webpMarker = new byte[] { 0x57, 0x45, 0x42, 0x50 };     // "WEBP"

    //                 bool isRiff = headerBytes.Take(4).SequenceEqual(riffSignature);
    //                 bool isWebp = headerBytes.Skip(8).Take(4).SequenceEqual(webpMarker);

    //                 return isRiff && isWebp;
    //             }

    //             return allowedSignatures.Any(signature =>
    //                 headerBytes.Take(signature.Length).SequenceEqual(signature));
    //         }
    //         finally
    //         {
    //             stream.Position = originalPosition;
    //         }
    //     }

    //     public override string ToString() => FileName;
    // }
}
