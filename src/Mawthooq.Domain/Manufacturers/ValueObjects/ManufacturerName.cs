using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Manufacturers.ValueObjects
{
    public record class ManufacturerName
    {
        public string Value { get; }

        public ManufacturerName(string manufacturerName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(manufacturerName, nameof(manufacturerName));

            manufacturerName = manufacturerName.Trim();

            if (manufacturerName.Length > 50)
                throw new InvalidOperationException("Manufacturer Name Cannot exceed 50 Characters");

            Value = CapitalizeFirstLetter(manufacturerName);
        }

        private static string CapitalizeFirstLetter(string input)
        {
            return char.ToUpper(input[0]) + input.AsSpan(1).ToString().ToLower();
        }
    }
}
