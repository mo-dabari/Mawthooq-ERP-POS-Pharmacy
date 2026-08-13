using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mawthooq.Domain.ValueObjects
{
    public record class Email
    {
        private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        public Email(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            string normalized = value.Trim().ToLowerInvariant();

            if (!EmailRegex.IsMatch(normalized))
                throw new ArgumentException("Invalid email format.");
            Value = normalized;
        }
    }
}
