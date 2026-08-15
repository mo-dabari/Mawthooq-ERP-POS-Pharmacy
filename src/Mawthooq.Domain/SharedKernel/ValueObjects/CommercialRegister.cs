using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.ValueObjects
{
    public record class CommercialRegister
    {
        public string Value { get; }

        public CommercialRegister(string commercialRegister)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(commercialRegister, nameof(commercialRegister));

            string cleanCommercialRegister = commercialRegister.Trim();

            if (!cleanCommercialRegister.All(char.IsDigit))

                throw new InvalidOperationException("The commercial registration number must contain only numbers.");


            if (cleanCommercialRegister.Length < 3 || cleanCommercialRegister.Length > 8)

                throw new InvalidOperationException("The commercial registration number is invalid in terms of length.");

                Value = commercialRegister;

        }
    }
}
