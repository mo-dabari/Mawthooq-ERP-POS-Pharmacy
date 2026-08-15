using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.LocalAgents.ValueObjects
{
    public record class LocalAgentName
    {
        public string Value { get; }

        public LocalAgentName(string localAgentName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(localAgentName, nameof(localAgentName));

            localAgentName = localAgentName.Trim();

            if (localAgentName.Length > 50)
                throw new InvalidOperationException("local Agent Name Cannot exceed 50 Characters");

            Value = CapitalizeFirstLetter(localAgentName);
        }

        private static string CapitalizeFirstLetter(string input)
        {
            return char.ToUpper(input[0]) + input.AsSpan(1).ToString().ToLower();
        }
    }
}
