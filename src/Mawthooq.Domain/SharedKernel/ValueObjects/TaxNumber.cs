using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.ValueObjects
{
    public record class TaxNumber
    {
        public string Value { get; }

        public TaxNumber(string taxNumber)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(taxNumber, nameof(taxNumber));

            string cleanTaxNumber = taxNumber.Replace("-", "").Replace(" ", "").Trim();

            if (cleanTaxNumber.Length > 9 || cleanTaxNumber.All(Char.IsDigit))
                throw new InvalidProgramException("The tax number must consist of exactly 9 digits.");

            Value = cleanTaxNumber;
        }


        public string ToFormattedString() => $"{Value[..3]}-{Value[3..6]}-{Value[..6]}";
    }
}
