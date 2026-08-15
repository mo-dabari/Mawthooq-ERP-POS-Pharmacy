using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.ActiveIngredients.ValueObjects
{
    public class ActiveIngredientName
    {
        public string Value { get; }

        public ActiveIngredientName(string activeIngredientName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(activeIngredientName, nameof(activeIngredientName));

            if (activeIngredientName.Length > 100)
                throw new InvalidProgramException("Active Ingredient name too long.");

            Value = activeIngredientName.Trim();
        }
    }
}
