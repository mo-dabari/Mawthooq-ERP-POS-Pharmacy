using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.ValueObjects.Barcodes
{
    public abstract record class Barcode
    {
        public const int MaxValueLength = 50;
        public string Value { get; }
        public BarcodeType Type { get; }

        protected Barcode(string value, BarcodeType type)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            if (!Enum.IsDefined(typeof(BarcodeType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(BarcodeType));

            Value = value.Trim();

            LengthGuard.MaxLength(Value, MaxValueLength, nameof(value));

            Type = type;
        }

        public override string ToString() => Value;
    }
}

