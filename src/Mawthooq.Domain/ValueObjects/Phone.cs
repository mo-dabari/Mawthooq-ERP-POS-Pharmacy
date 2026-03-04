using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mawthooq.Domain.ValueObjects
{
    public record class Phone
    {
        private static readonly Regex E164Regex =
        new(@"^\+[1-9]\d{7,14}$", RegexOptions.Compiled);

        public string Value { get; }

        public Phone(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            value = value.Trim();
            if (!E164Regex.IsMatch(value))
                throw new ArgumentException(
                    "Phone number must be in valid E.164 format. Example: +14155552671");

            Value = value;
        }
    }
}
