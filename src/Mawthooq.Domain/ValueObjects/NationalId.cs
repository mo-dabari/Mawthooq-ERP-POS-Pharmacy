using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects.Enums;

namespace Mawthooq.Domain.ValueObjects.NationalID
{
    public record class NationalId
    {

        private const RegexOptions _regexOptions = RegexOptions.Compiled;
        private static readonly Dictionary<CountryCode, Regex> _allowedPatterns = new()
        {
            { CountryCode.EG, new Regex(@"^\d{14}$", _regexOptions) },
            { CountryCode.SA, new Regex(@"^\d{10}$", _regexOptions) },
            { CountryCode.AE, new Regex(@"^\d{15}$", _regexOptions) },
            { CountryCode.QA, new Regex(@"^\d{11}$", _regexOptions) },
            { CountryCode.KW, new Regex(@"^\d{12}$", _regexOptions) },
            { CountryCode.OM, new Regex(@"^\d{8}$", _regexOptions) },
            { CountryCode.BH, new Regex(@"^\d{9}$", _regexOptions) },
            { CountryCode.JO, new Regex(@"^\d{10}$", _regexOptions) }
        };
        public CountryCode CountryCode { get; }
        public string Value { get;}

        public NationalId(CountryCode countryCode, string value)
        {
            if (!Enum.IsDefined(typeof(CountryCode), countryCode))
                throw new InvalidEnumArgumentException(nameof(countryCode), (int)countryCode, typeof(CountryCode));

            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            string normalized = value.Trim();

            if (!_allowedPatterns[countryCode].IsMatch(normalized))
                throw new ArgumentException($"The provided national ID does not match the allowed pattern for country code {countryCode}.", nameof(value));

            CountryCode = countryCode;
            Value = normalized;
        }
    }
}
