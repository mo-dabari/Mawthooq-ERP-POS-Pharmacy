using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.ActiveIngredients.ValueObjects
{
    public class ActiveIngredientDescription
    {
        public string Value { get; }

        public ActiveIngredientDescription(string activeIngredientDescription)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(activeIngredientDescription, nameof(activeIngredientDescription));

            if (activeIngredientDescription.Length > 200)
                throw new InvalidProgramException("Active Ingredient description too long.");

            Value = activeIngredientDescription.Trim();
        }
    }
}
