using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mawthooq.Domain.ValueObjects.NationalID
{
    public record class NationalId
    {
        private Regex AllowedPattern;

        public string Value { get; }

        public NationalId(Regex allowedPattern, string value)
        {
            ArgumentNullException.ThrowIfNull(allowedPattern, nameof(allowedPattern));
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            var normalized = value.Trim().ToUpperInvariant();

            if (!allowedPattern.IsMatch(normalized))
                throw new ArgumentException(
                    "National ID does not match the required format");

            AllowedPattern = allowedPattern;
            Value = normalized;
        }

        public virtual bool Equals(NationalId? other)
        {
            if (other is null) return false;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
