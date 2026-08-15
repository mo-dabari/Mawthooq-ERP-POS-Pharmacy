using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Domain.SharedKernel.ValueObjects
{
    public record class Phone
    {
        private const RegexOptions _options = RegexOptions.Compiled;
        private static readonly Dictionary<CountryCode, Regex> _patterns = new()
    {
        // Egypt
        { CountryCode.EG, new Regex(@"^01[0125]\d{8}$", _options) },

        // Saudi Arabia
        { CountryCode.SA, new Regex(@"^05\d{8}$", _options  ) },

        // UAE
        { CountryCode.AE, new Regex(@"^05\d{8}$", _options) },

        // Qatar
        { CountryCode.QA, new Regex(@"^[3-7]\d{7}$", _options) },

        // Kuwait
        { CountryCode.KW, new Regex(@"^[569]\d{7}$", _options) },

        // Oman
        { CountryCode.OM, new Regex(@"^9\d{7}$", _options) },

        // Bahrain
        { CountryCode.BH, new Regex(@"^3\d{7}$", _options) },

        // Jordan
        { CountryCode.JO, new Regex(@"^07\d{8}$", _options) },

        // Palestine
        { CountryCode.PS, new Regex(@"^05[69]\d{7}$", _options) }
    };
        public CountryCode CountryCode { get; }
        public string Value { get; }
        public PhoneType PhoneType { get; }

        public Phone(CountryCode countryCode, string value, PhoneType phoneType = PhoneType.Primary)
        {
            if (!Enum.IsDefined(typeof(CountryCode), countryCode))
                throw new InvalidEnumArgumentException(nameof(countryCode), (int)countryCode, typeof(CountryCode));

            if (!Enum.IsDefined(typeof(PhoneType), phoneType))
                throw new InvalidEnumArgumentException(nameof(phoneType), (int)phoneType, typeof(PhoneType));

            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            string normalized = value.Trim();

            if (!_patterns[countryCode].IsMatch(normalized))
                throw new ArgumentException($"The provided phone number does not match the allowed pattern for country code {countryCode}.", nameof(value));

            Value = normalized;
            CountryCode = countryCode;
            PhoneType = phoneType;
        }
        public override string ToString() => $"+{(int)CountryCode}{Value}";
    }
}
