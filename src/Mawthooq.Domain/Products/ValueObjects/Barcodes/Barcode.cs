using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Barcodes
{
    public abstract record class Barcode
    {
        public string Value { get; }
        public BarcodeType Type { get; }

        protected Barcode(string value, BarcodeType type)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            if (!Enum.IsDefined(typeof(BarcodeType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(BarcodeType));

            Value = value.Trim();
            Type = type;
        }

        public override string ToString() => Value;
    }
}

