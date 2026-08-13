using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Products.ValueObjects
{
    public sealed record class SKU
    {
        public string Value { get; }

        public SKU(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            value = value.Trim().ToUpperInvariant();

            if (value.Length > 50 || value.Length < 8)
                throw new ArgumentException("SKU must be between 8 and 50 characters.", nameof(value));

            if (!Regex.IsMatch(value, @"^[A-Z0-9\-]+$"))
                throw new ArgumentException("SKU can only contain letters, numbers, and hyphens.", nameof(value));

            Value = value;
        }

        public override string ToString() => Value;
    }
}
