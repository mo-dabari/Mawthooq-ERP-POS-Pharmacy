using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Medicines.Enums;

namespace Mawthooq.Domain.Medicines.ValueObjects
{
    public record class IngredientAmount
    {
        public decimal Value { get; }
        public IngredientAmountUnits Unit { get; }

          public IngredientAmount(decimal ingredientAmount, IngredientAmountUnits unit)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ingredientAmount, nameof(ingredientAmount));

            if (!Enum.IsDefined(typeof(IngredientAmountUnits), unit))
                throw new InvalidEnumArgumentException(nameof(unit), (int)unit, typeof(IngredientAmountUnits));

            Value = ingredientAmount;
            Unit = unit;
        }

            public override string ToString() => $"{Value} {Unit}";
    }
}
