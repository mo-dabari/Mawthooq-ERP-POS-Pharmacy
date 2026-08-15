using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Medicines.ValueObjects
{
    public record class MedicineName
    {
        public string Value { get; }

        public MedicineName(string medicineName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(medicineName);

            medicineName = medicineName.Trim();

            if (medicineName.Length > 15)
                throw new InvalidOperationException("Trade name length cannot exceed 15 characters");

            Value = CapitalizeFirstLetter(medicineName);
        }

        private static string CapitalizeFirstLetter(string input)
        {
            return char.ToUpper(input[0]) + input.AsSpan(1).ToString().ToLower();
        }
    }
}
